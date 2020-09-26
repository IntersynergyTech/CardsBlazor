using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class PaymentMethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSettleMatch",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SettleAuditId",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "PaymentAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositivePlayerId = table.Column<int>(type: "int", nullable: false),
                    NegativePlayerId = table.Column<int>(type: "int", nullable: false),
                    AmountTransferred = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SettleMatchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentAudits_Players_NegativePlayerId",
                        column: x => x.NegativePlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId");
                    table.ForeignKey(
                        name: "FK_PaymentAudits_Players_PositivePlayerId",
                        column: x => x.PositivePlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId");
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "ArchiveTime", "Archived", "HasFixedFee", "MinimumPlayerCount", "Name", "NumberOfWinnersInt" },
                values: new object[] { 999, null, false, false, 2, "Settlement", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SettleAuditId",
                table: "Matches",
                column: "SettleAuditId",
                unique: true,
                filter: "[SettleAuditId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAudits_Id",
                table: "PaymentAudits",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAudits_NegativePlayerId",
                table: "PaymentAudits",
                column: "NegativePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAudits_PositivePlayerId",
                table: "PaymentAudits",
                column: "PositivePlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PaymentAudits_SettleAuditId",
                table: "Matches",
                column: "SettleAuditId",
                principalTable: "PaymentAudits",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PaymentAudits_SettleAuditId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "PaymentAudits");

            migrationBuilder.DropIndex(
                name: "IX_Matches_SettleAuditId",
                table: "Matches");

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 999);

            migrationBuilder.DropColumn(
                name: "IsSettleMatch",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SettleAuditId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Games");
        }
    }
}
