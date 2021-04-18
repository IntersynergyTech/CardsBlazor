using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class AppleAuthPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApiKey",
                table: "AppleAuthUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "AppleAuthUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppleAuthUsers_PlayerId",
                table: "AppleAuthUsers",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppleAuthUsers_Players_PlayerId",
                table: "AppleAuthUsers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppleAuthUsers_Players_PlayerId",
                table: "AppleAuthUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppleAuthUsers_PlayerId",
                table: "AppleAuthUsers");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "AppleAuthUsers");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "AppleAuthUsers");
        }
    }
}
