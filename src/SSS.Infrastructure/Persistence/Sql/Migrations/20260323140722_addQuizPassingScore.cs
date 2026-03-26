using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class addQuizPassingScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PassingScore",
                table: "As_Quizzes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassingScore",
                table: "As_Quizzes");
        }
    }
}
