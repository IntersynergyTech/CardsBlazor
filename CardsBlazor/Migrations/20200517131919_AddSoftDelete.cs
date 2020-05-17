using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class AddSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArchiveTime",
                table: "Players",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchiveTime",
                table: "Participants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Participants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchiveTime",
                table: "Matches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchiveTime",
                table: "Games",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchiveTime",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ArchiveTime",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ArchiveTime",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ArchiveTime",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Games");
        }
    }
}
