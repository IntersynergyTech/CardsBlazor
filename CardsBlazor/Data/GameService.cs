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
    public class GameService
    {
        private readonly CardsAppContext _context;
        public GameService(CardsAppContext context)
        {
            _context = context;
        }

        public Game GetGame(int playerId)
        {
            return _context.Games.Find(playerId);
        }

        public async Task<List<Game>> GetPlayers()
        {
            return await _context.Games.OrderBy(x => x.GameId).ToListAsync().ConfigureAwait(true);
        }


        public IQueryable<Game> GetAllAsQueryable()
        {
            return _context.Games.AsQueryable();
        }

        public async Task<List<GameViewModel>> GetAllAsViewModels()
        {
            var returnable = new List<GameViewModel>();
            foreach (var test in _context.Games.Include(x => x.Matches).ThenInclude(x => x.Participants))
            {
                try
                {
                    var vm = new GameViewModel(test);
                    returnable.Add(vm);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return returnable;
        }

        public List<GameChooseModel> GetAllAsChooseModels()
        {
            var games = _context.Games
                .Where(x => !x.Archived)
                .Select(x => new GameChooseModel {GameId = x.GameId, Name = x.Name})
                .ToList();
            return games;
        }
    }
}
