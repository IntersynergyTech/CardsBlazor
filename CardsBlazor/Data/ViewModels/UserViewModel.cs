using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Areas.Identity.Data;

namespace CardsBlazor.Data.ViewModels
{
    public class UserViewModel
    {
        public string UserId;
        public string UserName;
        public List<int> Players;

        public UserViewModel(AppUser appUser)
        {
            UserId = appUser.Id;
            UserName = appUser.UserName;
            Players = new List<int>(appUser.Players.Select(x => x.PlayerId));
        }
    }
}
