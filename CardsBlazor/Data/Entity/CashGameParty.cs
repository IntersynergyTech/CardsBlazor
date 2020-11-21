namespace CardsBlazor.Data.Entity
{
    public class CashGameParty
    {
        public int PartyId { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public decimal AmountStaked { get; set; }
        public decimal? CashOutAmount { get; set; }

        /// <summary>
        /// Set to true if the player has left the game, cash out amount is therefore fixed
        /// </summary>
        public bool IsPlayerFinished { get; set; }

        public int GameId { get; set; }
        public CashGame Game { get; set; }
    }
}