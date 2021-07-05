using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CardsBlazor.Data.Entity
{
    public class PaymentAudit
    {
        public int Id { get; set; }
        public int PositivePlayerId { get; set; }
        public virtual Player PositivePlayer { get; set; }

        public int NegativePlayerId { get; set; }
        public virtual Player NegativePlayer { get; set; }

        public decimal AmountTransferred { get; set; }
        public DateTime PaymentDate { get; set; }

        public int SettleMatchId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public virtual Match SettleMatch { get; set; }
    }
}
