using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameOfThronePool.Migrations
{
    public partial class AddUserQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBonusQuestion",
                columns: table => new
                {
                    UserBonusQuestionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    QuestionNumber = table.Column<int>(nullable: false),
                    QuestionText = table.Column<string>(nullable: true),
                    QuestionAnswer = table.Column<string>(nullable: true),
                    Correct = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBonusQuestion", x => x.UserBonusQuestionID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBonusQuestion");
        }
    }
}
