﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class AddWinnerField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "Participants",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
#pragma warning disable CA1062
            migrationBuilder.DropColumn(
#pragma warning restore CA1062
                name: "IsWinner",
                table: "Participants");
        }
    }
}
