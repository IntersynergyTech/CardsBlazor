using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.Data.Entity
{
    public class CardsAppContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public CardsAppContext(DbContextOptions<CardsAppContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasIndex(x => x.PlayerId);
            modelBuilder.Entity<Player>().HasKey(x => x.PlayerId);
            modelBuilder.Entity<Player>().HasData(new Player
            {
                Archived = false,
                ArchiveTime = null,
                EmailAddress = "test@example.com",
                HasAdminPermission = true,
                LastPaid = null,
                Password = "asd123asd",
                PlayerId = 1,
                RealName = "Admin",
                UserName = "Admin"
            });

            modelBuilder.Entity<Match>().HasKey(x => x.MatchId);
            modelBuilder.Entity<Match>().HasOne(x => x.Game).WithMany(x => x.Matches);

            modelBuilder.Entity<Participant>().HasKey(x => x.ParticipantId);
            modelBuilder.Entity<Participant>().HasOne(x => x.Match).WithMany(x => x.Participants);
            modelBuilder.Entity<Participant>().HasOne(x => x.Player).WithMany(x => x.MatchesParticipatedIn);

            modelBuilder.Entity<Game>().HasKey(x => x.GameId);
            modelBuilder.Entity<Game>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Game>().Property(x => x.HasFixedFee).IsRequired();
            modelBuilder.Entity<Game>().Property(x => x.MinimumPlayerCount).IsRequired();
            modelBuilder.Entity<Game>().HasData(new Game
            {
                GameId = 1,
                Archived = false,
                Name = "Spin",
                HasFixedFee = false,
                MinimumPlayerCount = 2,
                NumberOfWinnersInt = 1,
                ArchiveTime = null
            });
        }
    }
}
