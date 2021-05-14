using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
    [SuppressMessage("ReSharper", "CA2227"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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
        public decimal EntranceFee { get; set; }
        public virtual List<Participant> Participants { get; set; }
        public bool Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }

        public bool IsResolved { get; set; }

        public decimal StakePerPoint { get; set; }

        public string MatchNotes { get; set; }

        public virtual List<MatchAudit> Audits { get; set; }

        public virtual PaymentAudit SettleAudit { get; set; }
        public int? SettleAuditId { get; set; }
        public bool IsSettleMatch { get; set; }
    }
}
