using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data.Entity;

namespace CardsBlazor.Data.ViewModels
{
    public class BoardViewModel
    {
        public DateTime TimeOfBoard { get; set; }
        public Dictionary<Player, decimal> Positions { get; set; }
    }
}
