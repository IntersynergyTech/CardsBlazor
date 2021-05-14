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
        public BoardApiModel()
        {
            
        }

        public BoardApiModel(List<BoardModel> positions, List<PlayerApiModel> players)
        {
            Players = players;
            Positions = positions;
        }
        public List<PlayerApiModel> Players { get; }
        public List<BoardModel> Positions { get; }
    }

    public class BoardModel
    {
        public int BoardModelId { get; set; }
        public DateTime PositionDate { get; set; }
        public Dictionary<int, decimal> PlayerPositions { get; set; }
    }
    
}