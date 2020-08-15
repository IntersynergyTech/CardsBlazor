using System;
using CardsBlazor.Areas.Identity.Data;
using CardsBlazor.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CardsBlazor.Areas.Identity.IdentityHostingStartup))]
namespace CardsBlazor.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IdentityContextConnection")));

                services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddUserManager<UserManager<AppUser>>()
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            });
        }
    }
    public class InitialApplicationState
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public string XsrfToken { get; set; }
    }
    public class TokenProvider
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken { get; set; }
    }
}