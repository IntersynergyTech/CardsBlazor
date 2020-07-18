using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public DateTime? LastPaid { get; set; }
        public bool HasAdminPermission { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual List<Participant> MatchesParticipatedIn { get; set; }
        public bool Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }

        [NotMapped]
        public decimal CurrentPosition
        {
            get
            {
                if (MatchesParticipatedIn == null || MatchesParticipatedIn.Count == 0) return 0m;
                var matchesSinceLastPaid =
                    MatchesParticipatedIn.Where(x => x.Match.IsResolved && !x.Archived && x.Match.EndTime >= LastPaid.GetValueOrDefault(DateTime.MinValue));
                var netResult = matchesSinceLastPaid.Sum(x => x.NetResult);
                return netResult.GetValueOrDefault(0);
            }
        }
    }
}
