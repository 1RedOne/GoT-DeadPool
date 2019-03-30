using Microsoft.EntityFrameworkCore.Migrations;

namespace GameOfThronePool.Migrations
{
    public partial class AddingPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCharacterSelection_AspNetUsers_ApplicationUserId",
                table: "UserCharacterSelection");

            migrationBuilder.DropIndex(
                name: "IX_UserCharacterSelection_ApplicationUserId",
                table: "UserCharacterSelection");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserCharacterSelection");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserCharacterSelection",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCharacterSelection_ApplicationUserId",
                table: "UserCharacterSelection",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCharacterSelection_AspNetUsers_ApplicationUserId",
                table: "UserCharacterSelection",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
