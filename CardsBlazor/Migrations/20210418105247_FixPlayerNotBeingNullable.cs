using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations
{
    public partial class FixPlayerNotBeingNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppleAuthUsers_Players_PlayerId",
                table: "AppleAuthUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "AppleAuthUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AppleAuthUsers_Players_PlayerId",
                table: "AppleAuthUsers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppleAuthUsers_Players_PlayerId",
                table: "AppleAuthUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "AppleAuthUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppleAuthUsers_Players_PlayerId",
                table: "AppleAuthUsers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
