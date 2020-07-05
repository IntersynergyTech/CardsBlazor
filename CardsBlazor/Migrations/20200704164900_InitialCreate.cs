using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimumPlayerCount = table.Column<int>(type: "int", nullable: false),
                    HasFixedFee = table.Column<bool>(type: "bit", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    ArchiveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfWinnersInt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastPaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasAdminPermission = table.Column<bool>(type: "bit", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    ArchiveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfPlayers = table.Column<int>(type: "int", nullable: false),
                    EntranceFee = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    ArchiveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchId);
                    table.ForeignKey(
                        name: "FK_Matches_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    ArchiveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetResult = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_Participants_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "Archived", "HasFixedFee", "MinimumPlayerCount", "Name", "NumberOfWinnersInt" },
                values: new object[] { 1, false, false, 2, "Spin", 1 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId",  "Archived", "EmailAddress", "HasAdminPermission", "Password", "RealName", "UserName" },
                values: new object[] { 1, false, "test@example.com", true, "asd123asd", "Admin", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GameId",
                table: "Matches",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_MatchId",
                table: "Participants",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PlayerId",
                table: "Participants",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerId",
                table: "Players",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
