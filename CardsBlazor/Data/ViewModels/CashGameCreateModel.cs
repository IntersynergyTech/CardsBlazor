using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Data.ViewModels
{
    public class CashGameCreateModel
    {
        [Required]
        public int? GameId { get; set; }

        [Required]
        [CustomValidator(ErrorMessage = "You must select at least 2 players")]
        public int[] StartingPlayers { get; set; }

        
        public string Stakes { get; set; }
    }
}
