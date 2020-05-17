using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
    public class Match
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int NumberOfPlayers { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal? EntranceFee { get; set; }
        public virtual List<Participant> Participants { get; set; }
        public bool Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }
    }
}
