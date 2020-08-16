using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
    public class MatchAudit
    {
        public int AuditId { get; set; }
        public int MatchId { get; set; }
        public string UserId { get; set; }
        public DateTime AuditDate { get; set; }
        public AuditType Type { get; set; }
        public virtual Match Match { get; set; }
    }

    public enum AuditType
    {
        CreateMatch,
        CompleteMatch,
        AddParty,
        RemoveParty,
        ArchiveMatch,
    }
}
