using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.ApiControllers;
using CardsBlazor.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.Data
{
    public class PlayerService
    {
        private readonly CardsAppContext _context;
        public PlayerService(CardsAppContext context)
        {
            _context = context;
        }

        public Player GetPlayer(int playerId)
        {
            return _context.Players.Find(playerId);
        }

        public async Task<List<Player>> GetPlayers()
        {
            return await _context.Players.OrderBy(x => x.PlayerId).ToListAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Gets all of the players in the form a view model
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlayerViewModel>> GetPlayersViewModel()
        {
            return await _context.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).OrderBy(x => x.PlayerId).Select(x => new PlayerViewModel(x)).ToListAsync().ConfigureAwait(true);
        }

        public IQueryable<Player> GetAllAsQueryable()
        {
            return _context.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).AsQueryable();
        }

        public void AddPlayer(PlayerViewModel player)
        {
            var model = new Player
            {
                UserName = player.UserName,
                EmailAddress = player.EmailAddress,
                Archived = false,
                HasAdminPermission = player.HasAdminPermission,
                RealName = player.RealName,
                LastPaid = player.LastPaid
            };
            _context.Players.Add(model);
            _context.SaveChanges();
        }

    }
}
