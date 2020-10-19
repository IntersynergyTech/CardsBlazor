using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data;
using CardsBlazor.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;

namespace CardsBlazor.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesController : ControllerBase
    {

        private readonly CardsAppContext _context;

        public FeesController(CardsAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="daysSincePlayed">Number of days a player is allowed to be inactive</param>
        /// <param name="percentageWagered">The percentage amount that a player must wager. For example 50 = 50%</param>
        /// <param name="percentageFee"></param>
        [Route("CalculateFees")]
        public void CalculateAndCheck(int daysSincePlayed, int percentageWagered, int percentageFee)
        {
            var players = _context.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).ToList();
            var lastDate = DateTime.Now.Date.AddDays(-daysSincePlayed);
            var boardUser = _context.Players.First(x => x.IsSystemInactiveUser);
            var feePerc = percentageFee / 100m;
            foreach (var player in players)
            {
                if (player.HideFromView || player.CurrentPosition == 0) continue;
                var matchesSince = player.MatchesParticipatedIn.Where(x => x.Match.EndTime > lastDate && x.IsResolved).ToList();
                var totalWagered = matchesSince.Sum(x => Math.Abs(x.NetResult ?? 0));
                var minimumStake = player.CurrentPosition * (percentageWagered / 100m);
                if (totalWagered > minimumStake) continue; //Player is active

                if (matchesSince.Where(x => x.Match.EndTime > DateTime.Now.Date.AddDays(-7))
                    .Any(x => x.Match.IsSettleMatch)) continue;

                Log.Logger.Information("Player {1} is inactive, removing board fee", player.RealName);
                var settleGame = CreateSettlementMatch(boardUser, player, player.CurrentPosition * feePerc);
            }
        }
        private Match CreateSettlementMatch(Player boardPlayer, Player inactivePlayer, decimal amount)
        {
            var settlementGame = _context.Games.Find(999);
            if (settlementGame == null)
            {
                Log.Logger.Error("Failed to seed database correctly, no settlement game type found");
                return null;
            }

            var trans = _context.Database.BeginTransaction();
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
            _context.Matches.Add(match);
            _context.SaveChanges();
            var winningParty = new Participant
            {
                IsResolved = true,
                IsWinner = true,
                NetResult = -amount,
                Archived = false,
                PlayerId = inactivePlayer.PlayerId,
                MatchId = match.MatchId //Winning party gets the amount removed from their position as they have been paid.
            };
            var losingParty = new Participant
            {
                IsResolved = true,
                IsWinner = false,
                NetResult = amount,
                Archived = false,
                PlayerId = boardPlayer.PlayerId,
                MatchId = match.MatchId
            };
            _context.Participants.Add(winningParty);
            _context.Participants.Add(losingParty);
            _context.SaveChanges();
            trans.Commit();
            trans.Dispose();
            return match;
        }
    }
}
