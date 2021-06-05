using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Analytics;
using CardsBlazor.ApiControllers;
using CardsBlazor.Data.Entity;
using CardsBlazor.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.Data
{
    public class BoardService
    {
        private readonly CardsAppContext _context;
        private readonly PlayerService _playerService;
        private readonly IAnalytics _analytic;
        
        public BoardService(CardsAppContext context, IAnalytics analytic)
        {
            _context = context;
            _analytic = analytic;
            _playerService = new PlayerService(_context, _analytic);
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
            return (from time in startTime let dict = 
                players.ToDictionary(player => player.PlayerId, player => player.GetPositionAtTime(time)) 
                select new BoardViewModel { Positions = dict, TimeOfBoard = time }).ToList();
        }

        public BoardApiModel GetNPositionsForApi(int take, int skip)
        {
            var timeList = _context.Matches.Where(x => !x.Archived && x.IsResolved).OrderByDescending(x => x.EndTime)
                .Skip(skip).Take(take).Select(x => x.EndTime.Value).ToList();
            var players = _context.Players.Where(x => !x.HideFromView)
                .Include(x => x.MatchesParticipatedIn)
                .ThenInclude(x => x.Match).ToList();

            var otherPlayers = _context.Players.Where(x => x.HideFromView)
                .Include(x => x.MatchesParticipatedIn)
                .ThenInclude(x => x.Match).ToList();
            var fakeUser = new PlayerApiModel()
            {
                Id = 42069,
                RealName = "Others",
                UserName = "Others",
                HideFromView = false
            };
            var playersForModel = _playerService.GetAllPlayersAsApiModel().Where(x => !x.HideFromView).ToList();
            playersForModel.Add(fakeUser);
            var returnable = new BoardApiModel
                (new List<BoardModel>(), playersForModel);
            for (int i = 0; i < timeList.Count; i++)
            {
                var time = timeList[i];
                var correctPlayers = players.GetPositionsAtTime(time);
                var otherDict = otherPlayers.GetPositionsAtTime(time);
                correctPlayers.Add(42069, otherDict.Sum(x => x.Value));
                returnable.Positions.Add(new BoardModel
                {
                    BoardModelId = i,
                    PositionDate = time,
                    PlayerPositions = correctPlayers
                });
            }

            return returnable;
        }


    }
}
