using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSurveyAnswerConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_As_SurveyAnswers_As_SurveyQuestions_QuestionId",
                table: "As_SurveyAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_As_SurveyAnswers_As_SurveyQuestions_QuestionId",
                table: "As_SurveyAnswers",
                column: "QuestionId",
                principalTable: "As_SurveyQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_As_SurveyAnswers_As_SurveyQuestions_QuestionId",
                table: "As_SurveyAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_As_SurveyAnswers_As_SurveyQuestions_QuestionId",
                table: "As_SurveyAnswers",
                column: "QuestionId",
                principalTable: "As_SurveyQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
