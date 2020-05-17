using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class NotMapped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Games_NumberOfWinners_Enum_Constraint",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "NumberOfWinners",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWinnersInt",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfWinnersInt",
                table: "Games");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Games_NumberOfWinners_Enum_Constraint",
                table: "Games",
                sql: "[NumberOfWinners] IN(1, 2)");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWinners",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
