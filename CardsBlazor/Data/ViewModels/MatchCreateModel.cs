using System.ComponentModel.DataAnnotations;

namespace CardsBlazor.Data.ViewModels
{
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