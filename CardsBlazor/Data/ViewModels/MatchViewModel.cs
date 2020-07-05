using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.ApiControllers;
using CardsBlazor.Data.Entity;

namespace CardsBlazor.Data.ViewModels
{
    public class MatchViewModel
    {
        public MatchViewModel()
        {

        }

        public MatchViewModel(Match matchData)
        {
            MatchId = matchData.MatchId;
            Game = matchData.Game;
            StartTime = matchData.StartTime;
            EndTime = matchData.EndTime;
            NumberOfPlayers = matchData.NumberOfPlayers;
            EntranceFee = matchData.EntranceFee;
        }
        public int MatchId { get; set; }
        public Game Game { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int NumberOfPlayers { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The stakes must be at least £1")]
        public decimal EntranceFee { get; set; }
        public List<Participant> Participants { get; set; }
    }

    public class MatchCreateModel
    {
        public MatchCreateModel()
        {

        }
        [Required]
        public int GameId { get; set; }

        [Required]
        public int[] StartingPlayers { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The stakes must be at least £1")]
        public decimal EntranceFee { get; set; }
    }
}
