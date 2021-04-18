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

        public DbSet<PaymentAudit> PaymentAudits { get; set; }

        public DbSet<MatchAudit> MatchAudits { get; set; }

        public DbSet<CashGame> CashGames { get; set; }
        public DbSet<CashGameParty> CashGameParties { get; set; }

        public DbSet<AppleAuthUser> AppleAuthUsers { get; set; }

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
            modelBuilder.Entity<Player>().Property(x => x.IsFeeUser).HasDefaultValue(false);
            modelBuilder.Entity<Player>().Property(x => x.IsSystemInactiveUser).HasDefaultValue(false);

            modelBuilder.Entity<Match>().HasKey(x => x.MatchId);
            modelBuilder.Entity<Match>().HasOne(x => x.Game).WithMany(x => x.Matches);
            modelBuilder.Entity<Match>().Property(x => x.IsSettleMatch).HasDefaultValue(false);

            modelBuilder.Entity<Participant>().HasKey(x => x.ParticipantId);
            modelBuilder.Entity<Participant>().HasOne(x => x.Match).WithMany(x => x.Participants);
            modelBuilder.Entity<Participant>().HasOne(x => x.Player).WithMany(x => x.MatchesParticipatedIn);

            modelBuilder.Entity<Game>().HasKey(x => x.GameId);
            modelBuilder.Entity<Game>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Game>().Property(x => x.HasFixedFee).IsRequired();
            modelBuilder.Entity<Game>().Property(x => x.MinimumPlayerCount).IsRequired();
            modelBuilder.Entity<Game>().Property(x => x.IsVisible).HasDefaultValue(true);
            modelBuilder.Entity<Game>().HasData(new Game
            {
                GameId = 1,
                Archived = false,
                Name = "Spin",
                HasFixedFee = true,
                MinimumPlayerCount = 2,
                NumberOfWinnersInt = 1,
                ArchiveTime = null
            }, new Game
            {
                GameId = 999,
                Archived = false,
                HasFixedFee = true,
                NumberOfWinnersInt = 1,
                ArchiveTime = null,
                MinimumPlayerCount = 2,
                Name = "Settlement"
            });

            modelBuilder.Entity<MatchAudit>().HasKey(x => x.AuditId);
            modelBuilder.Entity<MatchAudit>().Property(x => x.AuditDate).IsRequired();
            modelBuilder.Entity<MatchAudit>().HasOne(x => x.Match).WithMany(x => x.Audits);

            modelBuilder.Entity<PaymentAudit>().HasKey(x => x.Id);
            modelBuilder.Entity<PaymentAudit>().HasIndex(x => x.Id);
            modelBuilder.Entity<PaymentAudit>().HasOne(x => x.PositivePlayer).WithMany(x => x.PositivePayments)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<PaymentAudit>().HasOne(x => x.NegativePlayer).WithMany(x => x.NegativePayments)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<PaymentAudit>().Property(x => x.PaymentDate).IsRequired();
            modelBuilder.Entity<PaymentAudit>().HasOne(x => x.SettleMatch).WithOne(x => x.SettleAudit)
                .HasForeignKey<Match>(x => x.SettleAuditId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CashGame>().HasKey(x => x.CashGameId);
            modelBuilder.Entity<CashGame>().HasIndex(x => x.CashGameId);
            modelBuilder.Entity<CashGame>().Property(x => x.GameName).IsRequired();
            modelBuilder.Entity<CashGame>().HasMany(x => x.PartiesToGame).WithOne(x => x.Game)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CashGameParty>().HasKey(x => x.PartyId);
            modelBuilder.Entity<CashGameParty>().HasOne(x => x.Player);

            modelBuilder.Entity<AppleAuthUser>().HasKey(x => x.Id);
            modelBuilder.Entity<AppleAuthUser>().HasIndex(x => x.Id);
            modelBuilder.Entity<AppleAuthUser>().Property(x => x.AppleAuthCode).IsRequired();
            modelBuilder.Entity<AppleAuthUser>().HasOne(x => x.Player);
            modelBuilder.Entity<AppleAuthUser>().Property(x => x.DateArchived).HasColumnType("datetime");
        }
    }
}