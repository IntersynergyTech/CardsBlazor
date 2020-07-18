using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.ApiControllers;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CardsBlazor.Data
{
    public class MatchService : IDisposable
    {
        private readonly CardsAppContext _context;

        public MatchService(CardsAppContext context)
        {
            _context = context;
        }

        public Match GetMatch(int matchId)
        {
            return _context.Matches.Include(x => x.Game)
                .Include(x => x.Participants)
                .ThenInclude(x => x.Player)
                .FirstOrDefault(x => x.MatchId == matchId);
        }

        public List<Match> GetAllMatches()
        {
            return _context.Matches.Include(x => x.Game)
                .Include(x => x.Participants)
                .ThenInclude(x => x.Player).ToList();
        }

        public void AddPlayers(int matchId, List<int> playerIds)
        {
            var match = _context.Matches.Find(matchId);
            foreach (var player in playerIds)
            {
                var pl = _context.Players.Find(player);
                if (pl == null) throw new PlayerNotFoundException();
                var participant = new Participant
                {
                    Match = match,
                    MatchId = match.MatchId,
                    PlayerId = pl.PlayerId
                };
                _context.Participants.Add(participant);
            }

            match.NumberOfPlayers += playerIds.Count;
            _context.SaveChanges();
        }

        public void ResolveSingleWinnerMatch(int matchId, int winningParticipantId)
        {
            var trans = _context.Database.BeginTransaction();
            var match = _context.Matches.Include(x => x.Participants).Include(x => x.Game).FirstOrDefault(x => x.MatchId == matchId);
            if (match == null || match.Game.NumberOfWinners != NumberOfWinners.SingleWinner) throw new MatchNotFoundException();
            var winner = match.Participants.FirstOrDefault(x => x.ParticipantId == winningParticipantId);
            var totalWinnings = match.EntranceFee * match.Participants.Count(x => !x.Archived);
            winner.NetResult = totalWinnings - match.EntranceFee;
            winner.IsResolved = true;
            winner.IsWinner = true;
            foreach (var party in match.Participants.Where(x => x.ParticipantId != winner.ParticipantId && !x.IsResolved && !x.Archived))
            {
                party.NetResult = -match.EntranceFee;
                party.IsResolved = true;
                party.IsWinner = false;
            }

            match.IsResolved = true;
            match.EndTime = DateTime.Now;
            _context.SaveChanges();
            if (_context.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).ToList().Sum(x => x.CurrentPosition) != 0)
            {
                Log.Error("Board does not level out");
                throw new Exception("Board does not level");
            }
            trans.Commit();
            trans.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="playerResults">Dictionary containing each partyid and their net result</param>
        public void ResolveMultiWinnerMatch(int matchId, Dictionary<int, decimal> playerResults)
        {
            var trans = _context.Database.BeginTransaction();
            var valid = playerResults.Sum(x => x.Value) == 0;
            if (!valid) throw new NotImplementedException();
            var match = _context.Matches.Include(x => x.Participants).Include(x => x.Game).First(x => x.MatchId == matchId);
            foreach (var result in playerResults)
            {
                var party = match.Participants.First(x => x.ParticipantId == result.Key);
                party.IsWinner = result.Value >= 0;
                party.NetResult = result.Value;
                party.IsResolved = true;
                _context.SaveChanges();
            }

            match.IsResolved = true;
            match.EntranceFee = 0;
            match.EndTime = DateTime.Now;
            _context.SaveChanges();
            trans.Commit();
        }

        /// <summary>
        /// Returns a list of player view models that have not yet been registered in the tournament
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public List<PlayerViewModel> GetAvailablePlayers(int matchId)
        {
            var match = _context.Matches.Include(x => x.Participants).FirstOrDefault(x => x.MatchId == matchId && !x.Archived && !x.IsResolved);
            if (match == null) return new List<PlayerViewModel>();
            var removedPlayerIds = match.Participants.Where(x => !x.IsResolved).Select(x => x.PlayerId).ToList();
            return _context.Players.Where(x => !removedPlayerIds.Contains(x.PlayerId))
                .Select(x => new PlayerViewModel(x)).ToList();
        }

        public int AddNewMatch(MatchCreateModel createModel)
        {
            try
            {
                var trans = _context.Database.BeginTransaction();
                if (!createModel.GameId.HasValue)
                {
                    return -1;
                }

                var newMatch = new Match
                {
                    GameId = createModel.GameId.Value,
                    StartTime = DateTime.Now,
                    EntranceFee = createModel.EntranceFee
                };
                _context.Matches.Add(newMatch);
                _context.SaveChanges();
                foreach (var party in createModel.StartingPlayers)
                {
                    var partObj = new Participant
                    {
                        IsResolved = false,
                        MatchId = newMatch.MatchId,
                        PlayerId = party
                    };
                    _context.Participants.Add(partObj);
                }

                newMatch.NumberOfPlayers = createModel.StartingPlayers.Length;
                _context.SaveChanges();
                trans.Commit();
                trans.Dispose();
                return newMatch.MatchId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log.Error(e, "Error occured while saving a new match");
                return -1;
            }
        }

        public void ArchiveMatch(int matchId)
        {
            var match = _context.Matches.Include(x => x.Participants).FirstOrDefault(x => x.MatchId == matchId);
            if (match == null) return;
            foreach (var party in match.Participants)
            {
                party.IsResolved = true;
                party.ArchiveTime = DateTime.Now;
                party.Archived = true;
                party.NetResult = 0m;
            }

            match.ArchiveTime = DateTime.Now;
            match.Archived = true;
            match.EndTime = DateTime.Now;
            match.EntranceFee = 0m;
            match.IsResolved = true;

            _context.SaveChanges();
        }

        public void ArchivePlayer(int partyId)
        {
            var result = _context.Participants.Include(x => x.Match).FirstOrDefault(x => x.ParticipantId == partyId);
            if (result != null)
            {
                result.IsResolved = true;
                result.ArchiveTime = DateTime.Now;
                result.Archived = true;
                result.NetResult = 0m;
                result.Match.NumberOfPlayers--;
                _context.SaveChanges();
            }
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
    public class MatchNotFoundException : Exception { }
    public class IncorrectMatchTypeException : Exception { }

    public class PlayerNotFoundException : Exception
    {
    }
}
