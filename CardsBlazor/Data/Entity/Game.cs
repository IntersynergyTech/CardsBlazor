using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }
        public string Name { get; set; }
        public int MinimumPlayerCount { get; set; }

        /// <summary>
        /// This field describes if the game has a fixed entrance fee (Tournament Poker, Kalooki, Spins) or if the fee will change based on the outcome (Cash Poker, OFC)
        /// </summary>
        public bool HasFixedFee { get; set; }

        public virtual List<Match> Matches { get; set; }

        public bool Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }
        [NotMapped] 
        public NumberOfWinners NumberOfWinners => (NumberOfWinners) NumberOfWinnersInt;
        public int NumberOfWinnersInt { get; set; }
    }
}
