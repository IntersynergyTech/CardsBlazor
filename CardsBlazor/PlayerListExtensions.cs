using System;
using System.Collections.Generic;
using System.Linq;
using CardsBlazor.ApiControllers;
using CardsBlazor.Data.Entity;

namespace CardsBlazor
{
    public static class PlayerListExtensions
    {
        public static Dictionary<int, decimal> GetPositionsAtTime(this List<Player> players, DateTime time)
        {
            return players.ToDictionary(player => player.PlayerId, player => player.GetPositionAtTime(time));
        }
    }

    public class BoardApiModel
    {
        public List<PlayerApiModel> Players { get; set; }
        public List<BoardModel> Positions { get; set; }
    }

    public class BoardModel
    {
        public DateTime PositionDate { get; set; }
        public Dictionary<int, decimal> PlayerPositions { get; set; }
    }
}