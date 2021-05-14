using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data.Entity;

namespace CardsBlazor.Data.ViewModels
{
    [SuppressMessage("ReSharper", "CA2227"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class BoardViewModel
    {
        public DateTime TimeOfBoard { get; init; }
        public Dictionary<int, decimal> Positions { get; init; }
    }
}
