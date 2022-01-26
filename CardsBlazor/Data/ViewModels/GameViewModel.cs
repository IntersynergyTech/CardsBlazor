using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data.Entity;

namespace CardsBlazor.Data.ViewModels
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public int MinimumPlayerCount { get; set; }

        /// <summary>
        /// This field describes if the game has a fixed entrance fee (Tournament Poker, Kalooki, Spins) or if the fee will change based on the outcome (Cash Poker, OFC)
        /// </summary>
        public bool HasFixedFee { get; set; }

        public NumberOfWinners NumberOfWinners { get; set; }
        public int TotalNumberOfMatches { get; set; }
        public DateTime? LastPlayed { get; set; }
        public decimal TotalAmountWagered { get; set; }
        public decimal AverageStake { get; set; }

        public GameViewModel(Game entityGame)
        {
            GameId = entityGame.GameId;
            Name = entityGame.Name;
            MinimumPlayerCount = entityGame.MinimumPlayerCount;
            HasFixedFee = entityGame.HasFixedFee;
            NumberOfWinners = entityGame.NumberOfWinners;
            TotalNumberOfMatches = entityGame.Matches?.Count ?? 0;
            if (entityGame.Matches != null)
            {
                LastPlayed = entityGame.Matches.Any() ? entityGame.Matches.Max(x => x.StartTime) : DateTime.MinValue;
                TotalAmountWagered = entityGame.Matches.Any()
                    ? entityGame.Matches.Where(x => !x.Archived || !x.RemoveFromStatistics).SelectMany(x => x.Participants).Where(x => !x.Archived && x.IsResolved && x.NetResult >= 0)
                        .Sum(x => x.NetResult).GetValueOrDefault(0)
                    : 0;
                if (TotalNumberOfMatches > 0) AverageStake = TotalAmountWagered / TotalNumberOfMatches;
                else AverageStake = 0;
            }

        }

        public GameViewModel()
        {

        }
    }

    public class GameChooseModel
    {
        public int GameId { get; set; }
        public string Name { get; set; }
    }
}
