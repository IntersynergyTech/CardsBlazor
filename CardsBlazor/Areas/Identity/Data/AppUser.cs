using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CardsBlazor.Areas.Identity.Data
{
    [SuppressMessage("ReSharper", "CA2227"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    // Add profile data for application users by adding properties to the AppUser class
    public class AppUser : IdentityUser
    {
        public virtual List<AppUserJoiningPlayer> Players { get; set; }
    }
}
