using System.ComponentModel.DataAnnotations;

namespace CardsBlazor.Data.ViewModels
{
    public class MatchCreateModel
    {
        public MatchCreateModel()
        {

        }
        [Required]
        public int? GameId { get; set; }

        [Required]
        [CustomValidator(ErrorMessage = "You must select at least 2 players")]
        public int[] StartingPlayers { get; set; }

        [Required]
        [Range(1, 100000000, ErrorMessage = "The stakes must be at least £1 and at most £100000000")]
        public decimal EntranceFee { get; set; }
    }
    public class CustomValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var test = value as int[];
            if (test == null)
            {
                return ValidationResult.Success;
            }

            if (test.Length <= 1)
            {
                return new ValidationResult("You need at least 2 players to start a match");
            }

            return ValidationResult.Success;

        }
    }
}