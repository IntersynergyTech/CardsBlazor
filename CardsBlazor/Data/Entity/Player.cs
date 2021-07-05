using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        [Obsolete("LastPaid is no longer used due to an issue with multiple payments")]
        public DateTime? LastPaid { get; set; }
        public bool HasAdminPermission { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public virtual List<Participant> MatchesParticipatedIn { get; set; }
        public bool Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }
        public bool HideFromView { get; set; }
        /// <summary>
        /// True if the user is the board user. Used for fees
        /// </summary>
        public bool IsFeeUser { get; set; }

        public bool IsSystemInactiveUser { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public virtual List<PaymentAudit> PositivePayments { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public virtual List<PaymentAudit> NegativePayments { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public virtual List<CashGameParty> CashGamesPlayed { get; set; }

        [NotMapped]
        public decimal CurrentPosition
        {
            get
            {
                if (MatchesParticipatedIn == null || MatchesParticipatedIn.Count == 0) return 0m;
                var matchesSinceLastPaid =
                    MatchesParticipatedIn.Where(x => x.Match.IsResolved && !x.Archived);
                var netResult = matchesSinceLastPaid.Sum(x => x.NetResult);
                return netResult.GetValueOrDefault(0);
            }
        }

        public decimal GetPositionAtTime(DateTime date)
        {
            if (MatchesParticipatedIn == null || MatchesParticipatedIn.Count == 0) return 0m;
            var matchesSinceLastPaid =
                MatchesParticipatedIn.Where(x => x.Match.IsResolved && !x.Archived && x.Match.EndTime <= date);
            var netResult = matchesSinceLastPaid.Sum(x => x.NetResult);
            return netResult.GetValueOrDefault(0);
        }

        [NotMapped]
        public decimal PotentialPosition
        {
            get
            {
                var currentPosition = CurrentPosition;
                var currentSimpleStakes = MatchesParticipatedIn.Where(x => !x.IsResolved && !x.Archived && !x.Match.IsResolved && !x.Match.Archived)
                    .Sum(x => x.Match.EntranceFee);
                var currentCashGameStakes = CashGamesPlayed.Where(x => !x.IsPlayerFinished && !x.Game.IsArchived)
                    .Sum(x => x.AmountStaked);
                return currentPosition - (currentSimpleStakes + currentCashGameStakes);
            }
        }
    }
}
