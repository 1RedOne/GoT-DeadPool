using Microsoft.EntityFrameworkCore.Migrations;

namespace GameOfThronePool.Migrations
{
    public partial class FriendlyUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFriendlyName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFriendlyName",
                table: "AspNetUsers");
        }
    }
}
