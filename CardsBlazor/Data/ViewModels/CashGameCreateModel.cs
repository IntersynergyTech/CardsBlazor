using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data.Entity;

namespace CardsBlazor.Data.ViewModels
{
    public class CashGameCreateModel
    {
        [Required] public int? GameId { get; set; }

        [Required]
        [CustomValidator(ErrorMessage = "You must select at least 2 players")]
        public int[] StartingPlayers { get; set; }

        public string Stakes { get; set; }
    }

    public class CashGameViewModel
    {
        public DateTime StartTime { get; set; }
        public int CashGameId { get; set; }
        public string Stakes { get; set; }
        public int NumberOfPlayers { get; set; }
        public bool IsResolved { get; set; }

        public CashGameViewModel()
        {
        }


        public CashGameViewModel([NotNull] CashGame model)
        {
            CashGameId = model.CashGameId;
            Stakes = model.Stakes;
            IsResolved = model.IsFinished;
            StartTime = model.TimeStarted;
            NumberOfPlayers = model.PartiesToGame.Count;
        }
    }
}