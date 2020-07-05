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
    [Migration("20200705094212_AddWinnerField")]
    partial class AddWinnerField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0-preview.6.20312.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CardsBlazor.Data.Entity.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ArchiveTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<bool>("HasFixedFee")
                        .HasColumnType("bit");

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
                            HasFixedFee = false,
                            MinimumPlayerCount = 2,
                            Name = "Spin",
                            NumberOfWinnersInt = 1
                        });
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<int>("NumberOfPlayers")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MatchId");

                    b.HasIndex("GameId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("CardsBlazor.Data.Entity.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ArchiveTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasAdminPermission")
                        .HasColumnType("bit");

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
                            Password = "asd123asd",
                            RealName = "Admin",
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("CardsBlazor.Data.Entity.Match", b =>
                {
                    b.HasOne("CardsBlazor.Data.Entity.Game", "Game")
                        .WithMany("Matches")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                });
#pragma warning restore 612, 618
        }
    }
}
