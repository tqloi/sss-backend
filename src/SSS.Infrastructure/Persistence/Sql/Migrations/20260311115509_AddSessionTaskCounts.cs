using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionTaskCounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TasksCompletedCount",
                table: "Tr_StudySessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalTasks",
                table: "Tr_StudySessions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TasksCompletedCount",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "TotalTasks",
                table: "Tr_StudySessions");
        }
    }
}
