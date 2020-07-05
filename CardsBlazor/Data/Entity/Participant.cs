using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
    public class Participant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantId { get; set; }
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public bool Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? NetResult { get; set; }

        public bool IsResolved { get; set; }
    }
}
