using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.ApiControllers;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.Data
{
    public class MatchService
    {
        private readonly CardsAppContext _context;

        public MatchService(CardsAppContext context)
        {
            _context = context;
        }

        public Match GetMatch(int matchId)
        {
            return _context.Matches.Include(x => x.Participants).ThenInclude(x => x.Player).FirstOrDefault(x => x.MatchId == matchId);
        }

        public void AddPlayers(int matchId, int[] playerIds)
        {
            var match = _context.Matches.Find(matchId);
            foreach (var player in playerIds)
            {
                var pl = _context.Players.Find(player);
                var participant = new Participant
                {
                    Match = match,
                    MatchId = match.MatchId,
                    PlayerId = pl.PlayerId
                };
                _context.Participants.Add(participant);
            }

            _context.SaveChanges();
        }

        public void ResolveSingleWinnerMatch(int matchId, int winningParticipantId)
        {
            var match = _context.Matches.Include(x => x.Participants).Include(x => x.Game).FirstOrDefault(x => x.MatchId == matchId);
            if (match == null || match.Game.NumberOfWinners != NumberOfWinners.SingleWinner) throw new MatchNotFoundException();
            var winner = match.Participants.FirstOrDefault(x => x.ParticipantId == winningParticipantId);
            var totalWinnings = match.EntranceFee * match.Participants.Count;
            winner.NetResult = totalWinnings - match.EntranceFee;
            winner.IsResolved = true;
            foreach (var party in match.Participants.Where(x => x.ParticipantId != winner.ParticipantId && !x.IsResolved))
            {
                party.NetResult = -match.EntranceFee;
                party.IsResolved = true;
            }

            match.IsResolved = true;
            match.EndTime = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>
        /// Returns a list of player view models that have not yet been registered in the tournament
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public List<PlayerViewModel> GetAvailablePlayers(int matchId)
        {
            var match = _context.Matches.Include(x => x.Participants).FirstOrDefault(x => x.MatchId == matchId);
            var removedPlayerIds = match.Participants.Where(x => !x.IsResolved).Select(x => x.PlayerId).ToList();
            return _context.Players.Where(x => !removedPlayerIds.Contains(x.PlayerId))
                .Select(x => new PlayerViewModel(x)).ToList();
        }

        public int AddNewMatch(MatchCreateModel createModel)
        {
            var newMatch = new Match
            {
                GameId = createModel.GameId,
                StartTime = DateTime.Now,
                EntranceFee = createModel.EntranceFee
            };
            _context.Matches.Add(newMatch);
            _context.SaveChanges();
            return newMatch.MatchId;
        }
    }
    public class MatchNotFoundException : Exception{}
    public class IncorrectMatchTypeException : Exception{}
}
