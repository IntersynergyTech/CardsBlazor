using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.Data
{
    public class BoardService
    {
        private readonly CardsAppContext _context;

        public BoardService(CardsAppContext context)
        {
            _context = context;
        }

        public Dictionary<Player, decimal> GetAllPositionsAtTime(DateTime time)
        {
            var players = _context.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).ToList();
            var returnable = players.ToDictionary(player => player, player => player.GetPositionAtTime(time));
            return returnable;
        }

        public List<BoardViewModel> GetNPositions(int take, int skip)
        {
            var startTime = _context.Matches.Where(x => !x.Archived && x.IsResolved).OrderByDescending(x => x.EndTime)
                .Skip(skip).Take(take).Select(x => x.EndTime.Value).ToList();
            var players = _context.Players.Where(x => !x.HideFromView)
                .Include(x => x.MatchesParticipatedIn)
                .ThenInclude(x => x.Match).ToList();
            var hiddenPlayers =_context.Players.Where(x => x.HideFromView)
                .Include(x => x.MatchesParticipatedIn)
                .ThenInclude(x => x.Match).ToList();
            return (from time in startTime let dict = 
                players.ToDictionary(player => player.PlayerId, player => player.GetPositionAtTime(time)) 
                select new BoardViewModel { Positions = dict, TimeOfBoard = time }).ToList();
        }
    }
}
