using Microsoft.EntityFrameworkCore.Migrations;

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
            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "Participants");
        }
    }
}
