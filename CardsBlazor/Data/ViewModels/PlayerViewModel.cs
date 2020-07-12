using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CardsBlazor.Data.Entity;

namespace CardsBlazor.ApiControllers
{
    public class PlayerViewModel
    {
        [Key]
        [ReadOnly(true)]
        public int PlayerId { get; set; }
        [Key]
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string RealName { get; set; }
        public DateTime? LastPaid { get; set; }
        public bool HasAdminPermission { get; set; }

        public decimal CurrentPosition { get; set; }

        public PlayerViewModel(Player player)
        {
            if (player != null)
            {
                PlayerId = player.PlayerId;
                UserName = player.UserName;
                EmailAddress = player.EmailAddress;
                RealName = player.RealName;
                LastPaid = player.LastPaid;
                HasAdminPermission = player.HasAdminPermission;
                CurrentPosition = player.CurrentPosition;
            }

        }

        [JsonConstructor]
        public PlayerViewModel()
        {

        }
    }
}