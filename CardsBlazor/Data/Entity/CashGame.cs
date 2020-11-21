using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.Entity
{
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
