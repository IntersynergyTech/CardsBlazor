using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CardsBlazor.Data.Entity
{
    [SuppressMessage("ReSharper", "CA2227"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }
        public string Name { get; set; }
        public int MinimumPlayerCount { get; set; }

        /// <summary>
        /// This field describes if the game has a known buy in (Tournament Poker, Kalooki, Spins) or if the fee will change based on the outcome (OFC)
        /// </summary>
        public bool HasFixedFee { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public virtual List<Match> Matches { get; set; }

        public bool Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }
        [NotMapped] 
        public NumberOfWinners NumberOfWinners => (NumberOfWinners) NumberOfWinnersInt;
        public int NumberOfWinnersInt { get; set; }

        public bool IsVisible { get; set; }
        
    }
}
