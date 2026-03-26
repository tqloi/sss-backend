using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class addQuizQuestionLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "As_QuizQuestions",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "As_QuizQuestions");
        }
    }
}
