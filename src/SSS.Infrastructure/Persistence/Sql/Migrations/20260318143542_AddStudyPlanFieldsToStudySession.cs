using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddStudyPlanFieldsToStudySession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StudyPlanId",
                table: "Tr_StudySessions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudyPlanModuleId",
                table: "Tr_StudySessions",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudyPlanId",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "StudyPlanModuleId",
                table: "Tr_StudySessions");
        }
    }
}
