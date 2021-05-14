using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
    [SuppressMessage("ReSharper", "CA2227", Scope = "namespace", Target = "CardsBlazor.Data.Entity")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Scope = "namespace", Target = "CardsBlazor.Data.Entity")]
    public class CashGame
    {
        public int CashGameId { get; set; }
        public string GameName { get; set; }
        public string Stakes { get; set; }
        public bool IsFinished { get; set; }
        public DateTime TimeStarted { get; set; }
        public DateTime? TimeFinished { get; set; }
        public bool IsArchived { get; set; }
        public int? ArchivedBy { get; set; }
        public virtual List<CashGameParty> PartiesToGame { get; set; }

        [NotMapped]
        public decimal TotalStaked => PartiesToGame.Sum(x => x.AmountStaked);
    }
}
