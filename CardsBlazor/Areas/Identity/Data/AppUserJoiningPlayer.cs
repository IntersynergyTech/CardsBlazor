using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsBlazor.Areas.Identity.Data
{
    public class AppUserJoiningPlayer
    {
        public int UserPlayerJoinId { get; set; }
        public string UserId { get; set; }
        public int PlayerId { get; set; }

        public virtual AppUser User { get; set; }
    }
}
