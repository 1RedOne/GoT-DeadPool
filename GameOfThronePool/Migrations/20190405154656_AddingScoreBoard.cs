using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameOfThronePool.Migrations
{
    public partial class AddingScoreBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserScoreRecord",
                columns: table => new
                {
                    UserScoreRecordID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserFriendlyName = table.Column<string>(nullable: true),
                    BaseScore = table.Column<int>(nullable: false),
                    BonusScore = table.Column<bool>(nullable: false),
                    TotalScore = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScoreRecord", x => x.UserScoreRecordID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserScoreRecord");
        }
    }
}
