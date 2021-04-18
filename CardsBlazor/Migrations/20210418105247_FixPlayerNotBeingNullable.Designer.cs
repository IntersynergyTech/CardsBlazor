﻿// <auto-generated />
using System;
using CardsBlazor.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CardsBlazor.Migrations
{
    [DbContext(typeof(CardsAppContext))]
    [Migration("20210418105247_FixPlayerNotBeingNullable")]
    partial class FixPlayerNotBeingNullable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("CardsBlazor.Data.Entity.AppleAuthUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<Guid>("ApiKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppleAuthCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateArchived")
                        .HasColumnType("datetime");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAllowedAccess")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastAuth")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("AppleAuthUsers");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.CashGame", b =>
                {
                    b.Property<int>("CashGameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("ArchivedBy")
                        .HasColumnType("int");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<string>("Stakes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimeFinished")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStarted")
                        .HasColumnType("datetime2");

                    b.HasKey("CashGameId");

                    b.HasIndex("CashGameId");

                    b.ToTable("CashGames");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.CashGameParty", b =>
                {
                    b.Property<int>("PartyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("AmountStaked")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CashOutAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPlayerFinished")
                        .HasColumnType("bit");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("PartyId");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("CashGameParties");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("ArchiveTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<bool>("HasFixedFee")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("MinimumPlayerCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfWinnersInt")
                        .HasColumnType("int");

                    b.HasKey("GameId");

                    b.ToTable("Games");

                    b.HasData(
                        new
                        {
                            GameId = 1,
                            Archived = false,
                            HasFixedFee = true,
                            IsVisible = false,
                            MinimumPlayerCount = 2,
                            Name = "Spin",
                            NumberOfWinnersInt = 1
                        },
                        new
                        {
                            GameId = 999,
                            Archived = false,
                            HasFixedFee = true,
                            IsVisible = false,
                            MinimumPlayerCount = 2,
                            Name = "Settlement",
                            NumberOfWinnersInt = 1
                        });
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("ArchiveTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("EntranceFee")
                        .HasColumnType("decimal(8,2)");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<bool>("IsResolved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSettleMatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("MatchNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfPlayers")
                        .HasColumnType("int");

                    b.Property<int?>("SettleAuditId")
                        .HasColumnType("int");

                    b.Property<decimal>("StakePerPoint")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MatchId");

                    b.HasIndex("GameId");

                    b.HasIndex("SettleAuditId")
                        .IsUnique()
                        .HasFilter("[SettleAuditId] IS NOT NULL");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.MatchAudit", b =>
                {
                    b.Property<int>("AuditId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("AuditDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuditId");

                    b.HasIndex("MatchId");

                    b.ToTable("MatchAudits");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("ArchiveTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<bool>("IsResolved")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsWinner")
                        .HasColumnType("bit");

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<decimal?>("NetResult")
                        .HasColumnType("decimal(8,2)");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("ParticipantId");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.PaymentAudit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("AmountTransferred")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("NegativePlayerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PositivePlayerId")
                        .HasColumnType("int");

                    b.Property<int>("SettleMatchId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("NegativePlayerId");

                    b.HasIndex("PositivePlayerId");

                    b.ToTable("PaymentAudits");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("ArchiveTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasAdminPermission")
                        .HasColumnType("bit");

                    b.Property<bool>("HideFromView")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFeeUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsSystemInactiveUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastPaid")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RealName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            PlayerId = 1,
                            Archived = false,
                            EmailAddress = "test@example.com",
                            HasAdminPermission = true,
                            HideFromView = false,
                            IsFeeUser = false,
                            IsSystemInactiveUser = false,
                            Password = "asd123asd",
                            RealName = "Admin",
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.AppleAuthUser", b =>
                {
                    b.HasOne("CardsBlazor.Data.Entity.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.CashGameParty", b =>
                {
                    b.HasOne("CardsBlazor.Data.Entity.CashGame", "Game")
                        .WithMany("PartiesToGame")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CardsBlazor.Data.Entity.Player", "Player")
                        .WithMany("CashGamesPlayed")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Match", b =>
                {
                    b.HasOne("CardsBlazor.Data.Entity.Game", "Game")
                        .WithMany("Matches")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CardsBlazor.Data.Entity.PaymentAudit", "SettleAudit")
                        .WithOne("SettleMatch")
                        .HasForeignKey("CardsBlazor.Data.Entity.Match", "SettleAuditId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Game");

                    b.Navigation("SettleAudit");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.MatchAudit", b =>
                {
                    b.HasOne("CardsBlazor.Data.Entity.Match", "Match")
                        .WithMany("Audits")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Participant", b =>
                {
                    b.HasOne("CardsBlazor.Data.Entity.Match", "Match")
                        .WithMany("Participants")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CardsBlazor.Data.Entity.Player", "Player")
                        .WithMany("MatchesParticipatedIn")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.PaymentAudit", b =>
                {
                    b.HasOne("CardsBlazor.Data.Entity.Player", "NegativePlayer")
                        .WithMany("NegativePayments")
                        .HasForeignKey("NegativePlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CardsBlazor.Data.Entity.Player", "PositivePlayer")
                        .WithMany("PositivePayments")
                        .HasForeignKey("PositivePlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("NegativePlayer");

                    b.Navigation("PositivePlayer");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.CashGame", b =>
                {
                    b.Navigation("PartiesToGame");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Game", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Match", b =>
                {
                    b.Navigation("Audits");

                    b.Navigation("Participants");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.PaymentAudit", b =>
                {
                    b.Navigation("SettleMatch");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Player", b =>
                {
                    b.Navigation("CashGamesPlayed");

                    b.Navigation("MatchesParticipatedIn");

                    b.Navigation("NegativePayments");

                    b.Navigation("PositivePayments");
                });
#pragma warning restore 612, 618
        }
    }
}
