using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CardsBlazor.Areas.Identity.Data;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CardsBlazor.Data
{
    public class CashGameService : IDisposable
    {
        private CardsAppContext _context;
        private IHttpContextAccessor _httpContext;
        private UserManager<AppUser> _userManager;
        private MatchService _matchService;
        private int _cashGameGameId;

        public CashGameService(CardsAppContext context, IHttpContextAccessor httpContext, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContext = httpContext;
            _userManager = userManager;
            _matchService = new MatchService(_context, _httpContext, _userManager);
            _cashGameGameId = _context.Games.First(x => x.Name.Contains("Cash Poker")).GameId;
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IQueryable<CashGame> GetAllCashGames()
        {
            return _context.CashGames.Include(x => x.PartiesToGame).ThenInclude(x => x.Player).AsQueryable();
        }

        public CashGame GetCashGame(int id)
        {
            return _context.CashGames.Include(x => x.PartiesToGame).ThenInclude(x => x.Player)
                .FirstOrDefault(x => x.CashGameId == id);
        }

        public int StartCashGame(CashGameCreateModel model)
        {
            try
            {
                if (model == null) return -1;
                var trans = _context.Database.BeginTransaction();
                var game = new CashGame
                {
                    GameName = "Cash Poker",
                    Stakes = model.Stakes,
                    TimeStarted = DateTime.Now
                };
                _context.CashGames.Add(game);
                _context.SaveChanges();
                foreach (var player in model.StartingPlayers)
                {
                    var party = new CashGameParty
                    {
                        AmountStaked = 0m,
                        PlayerId = player,
                        GameId = game.CashGameId,
                        IsPlayerFinished = false
                    };
                    _context.CashGameParties.Add(party);
                }

                if (_httpContext.HttpContext != null)
                {
                    var userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var audit = new MatchAudit
                    {
                        AuditDate = DateTime.Now,
                        MatchId = game.CashGameId,
                        UserId = userId,
                        Type = AuditType.CreateMatch
                    };
                    _context.MatchAudits.Add(audit);
                }

                _context.SaveChanges();
                trans.Commit();
                return game.CashGameId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log.Error(e, "Error occured while saving a new match");
                return -1;
            }
        }

        public decimal AddBuyIn(int partyId, decimal buyInAmount)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    var party = _context.CashGameParties.Include(x => x.Game).FirstOrDefault(x => x.PartyId == partyId);
                    if (party == null) throw new NullReferenceException("Unable to find this party");
                    if (party.IsPlayerFinished || party.Game == null || party.Game.IsFinished || party.Game.IsArchived)
                        throw new NullReferenceException("This game does not exist or is finished");
                    party.AmountStaked += buyInAmount;
                    _context.SaveChanges();
                    trans.Commit();
                    return buyInAmount;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error occurred while adding a buy in");
                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="finalCashOutAmounts">PartyId, CashOutAmount</param>
        public void FinishMatch(int matchId, Dictionary<int, decimal> finalCashOutAmounts)
        {
            var cashGame = _context.CashGames.Include(x => x.PartiesToGame)
                .First(x => x.CashGameId == matchId);
            using (var trans = _context.Database.BeginTransaction())
            {
                
                foreach (var kvp in finalCashOutAmounts)
                {
                    var party = cashGame.PartiesToGame.First(x => x.PlayerId == kvp.Key);
                    party.CashOutAmount = kvp.Value;
                    party.IsPlayerFinished = true;
                }

                cashGame.IsFinished = true;
                _context.SaveChanges();
                trans.Commit();
            }

            var newMatchId = _matchService.AddNewMatch(new MatchCreateModel
            {
                EntranceFee = 1,
                GameId = _cashGameGameId,
                StartingPlayers = cashGame.PartiesToGame.Select(x => x.PlayerId).Distinct().ToArray()
            });
            if (newMatchId == 1)
            {
                throw new Exception();
            }

            var resolvedGame = _context.CashGames.Include(x => x.PartiesToGame).First(x => x.CashGameId == matchId);
            var match = _context.Matches.Include(x => x.Participants).First(x => x.MatchId == newMatchId);
            var finalResults = new Dictionary<int, decimal>();
            foreach (var party in resolvedGame.PartiesToGame.GroupBy(x => x.PlayerId))
            {
                var totalWinnings = party.Sum(x => x.CashOutAmount).GetValueOrDefault(0) - party.Sum(x => x.AmountStaked);
                var partyId = match.Participants.First(x => !x.IsResolved && !x.Archived && x.PlayerId == party.Key);
                finalResults.Add(partyId.ParticipantId, totalWinnings);
            }
            _matchService.ResolveMultiWinnerMatch(newMatchId, finalResults);

        }

        public void FinishEntry(int matchId, int partyId, decimal cashOutAmount)
        {
            var party = _context.CashGameParties.First(x => x.GameId == matchId && partyId == x.PartyId);
            party.CashOutAmount = cashOutAmount;
            party.IsPlayerFinished = true;
            _context.SaveChanges();
        }
    }
}
