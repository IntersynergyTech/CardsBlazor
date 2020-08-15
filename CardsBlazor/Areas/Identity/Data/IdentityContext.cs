using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.Data
{
    public class IdentityContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public DbSet<AppUserJoiningPlayer> UserJoinPlayer { get; set; }
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<AppUserJoiningPlayer>(x =>
            {
                x.HasKey(y => y.UserPlayerJoinId);
                x.HasOne(y => y.User).WithMany(y => y.Players);
                x.HasIndex(y => y.UserPlayerJoinId);
            });
        }
    }
}
