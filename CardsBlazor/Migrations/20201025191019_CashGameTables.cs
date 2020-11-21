using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class CashGameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashGames",
                columns: table => new
                {
                    CashGameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stakes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    TimeStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeFinished = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashGames", x => x.CashGameId);
                });

            migrationBuilder.CreateTable(
                name: "CashGameParties",
                columns: table => new
                {
                    PartyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    AmountStaked = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashOutAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPlayerFinished = table.Column<bool>(type: "bit", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashGameParties", x => x.PartyId);
                    table.ForeignKey(
                        name: "FK_CashGameParties_CashGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CashGames",
                        principalColumn: "CashGameId");
                    table.ForeignKey(
                        name: "FK_CashGameParties_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1,
                column: "HasFixedFee",
                value: true);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 999,
                column: "HasFixedFee",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashGameParties_GameId",
                table: "CashGameParties",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CashGameParties_PlayerId",
                table: "CashGameParties",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CashGames_CashGameId",
                table: "CashGames",
                column: "CashGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashGameParties");

            migrationBuilder.DropTable(
                name: "CashGames");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1,
                column: "HasFixedFee",
                value: false);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 999,
                column: "HasFixedFee",
                value: false);
        }
    }
}
