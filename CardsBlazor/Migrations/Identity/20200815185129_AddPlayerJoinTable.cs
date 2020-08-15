using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsBlazor.Migrations.Identity
{
    public partial class AddPlayerJoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserJoinPlayer",
                columns: table => new
                {
                    UserPlayerJoinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJoinPlayer", x => x.UserPlayerJoinId);
                    table.ForeignKey(
                        name: "FK_UserJoinPlayer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserJoinPlayer_UserId",
                table: "UserJoinPlayer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJoinPlayer_UserPlayerJoinId",
                table: "UserJoinPlayer",
                column: "UserPlayerJoinId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserJoinPlayer");
        }
    }
}
