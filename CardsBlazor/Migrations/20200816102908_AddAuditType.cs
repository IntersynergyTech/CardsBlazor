using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class AddAuditType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MatchAudits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MatchAudits");
        }
    }
}
