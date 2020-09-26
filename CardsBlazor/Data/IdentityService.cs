using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Areas.Identity.Data;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;

namespace CardsBlazor.Data
{
    public class IdentityService
    {
        private readonly CardsAppContext _cardsContext;
        private readonly IdentityContext _identityContext;
        public IdentityService(CardsAppContext cardsContext, IdentityContext identityContext)
        {
            _cardsContext = cardsContext;
            _identityContext = identityContext;
        }

        /// <summary>
        /// Returns the AppUser for a given PlayerId or null if the user or player isn't found
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public AppUser GetUserForPlayer(int playerId)
        {
            var player = _cardsContext.Players.Find(playerId);
            if (player == null) return null;
            var join = _identityContext.UserJoinPlayer.Include(x => x.User)
                .FirstOrDefault(x => x.PlayerId == player.PlayerId);
            return join?.User;
        }

        public void AddPlayerToUser(int playerId, string userId)
        {
            var player = _cardsContext.Players.Find(playerId);
            if (player == null) throw new ArgumentException("This player doesn't exist", nameof(playerId));
            var join = _identityContext.UserJoinPlayer.Any(x => x.PlayerId == playerId);
            if (join)
                throw new ArgumentException("This player already has a user attached. Remove the user first",
                    nameof(playerId));
            var user = _identityContext.Users.Find(userId);
            if (user == null) throw new ArgumentException("This user doesn't exist.", nameof(userId));
            var joining = new AppUserJoiningPlayer
            {
                PlayerId = playerId,
                User = user,
                UserId = userId
            };
            _identityContext.UserJoinPlayer.Add(joining);
            _identityContext.SaveChanges();
        }

        public void RemovePlayerToUserAssociation(int playerId, string userId)
        {
            var player = _cardsContext.Players.Find(playerId);
            if (player == null) throw new ArgumentException("This player doesn't exist", nameof(playerId));
            var join = _identityContext.UserJoinPlayer.Where(x => x.PlayerId == playerId && userId == x.UserId).ToList();
            if (!join.Any())
                throw new ArgumentException("This player doesn't have this user attached",
                    nameof(playerId));
            _identityContext.UserJoinPlayer.RemoveRange(join);
            _identityContext.SaveChanges();
        }

        public List<UserViewModel> GetUsers()
        {
            return _identityContext.Users.Include(x => x.Players).Select(x => new UserViewModel(x)).ToList();
        }

        public UserViewModel GetPlayerModel(string userName)
        {
            return _identityContext.Users.Include(x => x.Players).Select(x => new UserViewModel(x))
                .FirstOrDefault(x => x.UserName == userName);
        }

        public PaymentAudit RecordPayment(int positivePlayerId, int negativePlayerId, decimal amount)
        {
            
            var positivePlayer = _cardsContext.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).FirstOrDefault(x => x.PlayerId == positivePlayerId);
            var negativePlayer = _cardsContext.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).FirstOrDefault(x => x.PlayerId == negativePlayerId);
            if (positivePlayer == null) throw new ArgumentException("This player doesn't exist", nameof(positivePlayerId));
            if (negativePlayer == null) throw new ArgumentException("This player doesn't exist", nameof(negativePlayerId));
            Log.Logger.Information($"Recording payment of {amount:C2} from {negativePlayerId} to {positivePlayerId}");

            var match = CreateSettlementMatch(positivePlayer, negativePlayer, amount);
            var audit = new PaymentAudit
            {
                AmountTransferred = amount,
                NegativePlayerId = negativePlayer.PlayerId,
                NegativePlayer = negativePlayer,
                SettleMatch = match,
                PaymentDate = DateTime.Now,
                PositivePlayerId = positivePlayer.PlayerId,
                PositivePlayer = positivePlayer,
                SettleMatchId = match.MatchId
            };
            _cardsContext.PaymentAudits.Add(audit);
            _cardsContext.SaveChanges();
            return audit;
        }

        private Match CreateSettlementMatch(Player positivePlayer, Player negativePlayer, decimal amount)
        {
            var settlementGame = _cardsContext.Games.Find(999);
            if (settlementGame == null)
            {
                Log.Logger.Error("Failed to seed database correctly, no settlement game type found");
                return null;
            }

            var trans = _cardsContext.Database.BeginTransaction();
            var match = new Match
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                IsSettleMatch = true,
                IsResolved = true,
                EntranceFee = amount,
                Archived = false,
                GameId = settlementGame.GameId,
            };
            _cardsContext.Matches.Add(match);
            _cardsContext.SaveChanges();
            var winningParty = new Participant
            {
                IsResolved = true,
                IsWinner = true,
                NetResult = -amount,
                Archived = false,
                PlayerId = positivePlayer.PlayerId,
                MatchId = match.MatchId //Winning party gets the amount removed from their position as they have been paid.
            };
            var losingParty = new Participant
            {
                IsResolved = true,
                IsWinner = false,
                NetResult = amount,
                Archived = false,
                PlayerId = negativePlayer.PlayerId,
                MatchId = match.MatchId
            };
            _cardsContext.Participants.Add(winningParty);
            _cardsContext.Participants.Add(losingParty);
            _cardsContext.SaveChanges();
            trans.Commit();
            trans.Dispose();
            return match;
        }
    }
}
