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
            Participants = new List<PartyViewModel>();
            Game = new GameViewModel();
        }

        public MatchViewModel(Match matchData)
        {
            if (matchData == null) return;
            MatchId = matchData.MatchId;
            Game = new GameViewModel(matchData.Game);
            StartTime = matchData.StartTime;
            EndTime = matchData.EndTime;
            NumberOfPlayers = matchData.NumberOfPlayers;
            EntranceFee = matchData.EntranceFee;
            IsResolved = matchData.IsResolved;


            if (matchData.Participants != null && matchData.Participants.Count > 0)
            {
                Participants = new List<PartyViewModel>(matchData.Participants.Select(x => new PartyViewModel(x)));
            }
            else
            {
                Participants = new List<PartyViewModel>();
            }
        }
        public int MatchId { get; set; }
        public GameViewModel Game { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int NumberOfPlayers { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The stakes must be at least £1")]
        public decimal EntranceFee { get; set; }
        public List<PartyViewModel> Participants { get; set; }
        public bool IsResolved { get; set; }
        public string GameName => Game?.Name;
    }

    public class PartyViewModel
    {
        public PartyViewModel(Participant party)
        {
            ParticipantId = party.ParticipantId;
            MatchId = party.MatchId;
            PlayerId = party.PlayerId;
            NetResult = party.NetResult;
            IsResolved = party.IsResolved;
            IsWinner = party.IsWinner;
            RealName = party.Player.RealName;
        }

        public PartyViewModel()
        {

        }
        public int ParticipantId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public decimal? NetResult { get; set; }
        public bool IsResolved { get; set; }
        public bool? IsWinner { get; set; }
        public string RealName { get; set; }
    }
}
