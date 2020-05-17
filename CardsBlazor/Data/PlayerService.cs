using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return await _context.Players.ToListAsync();
        }

        public IQueryable<Player> GetAllAsQueryable()
        {
            return _context.Players.AsQueryable();
        }

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

    }
}
