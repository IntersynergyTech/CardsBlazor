using System;

namespace CardsBlazor.Data.Entity
{
    public class AppleAuthUser
    {
        public int Id { get; set; }
        public string AppleAuthCode { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAllowedAccess { get; set; }
        public DateTime? LastAuth { get; set; }
        public DateTime? DateArchived { get; set; }
        public Guid ApiKey { get; set; }
        public Player Player { get; set; }
        public int? PlayerId { get; set; }
    }
}