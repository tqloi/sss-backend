using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStudySessionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveSeconds",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "ConfidenceActiveLearning",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "FatigueScore",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "FocusScore",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "IdleSeconds",
                table: "Tr_StudySessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveSeconds",
                table: "Tr_StudySessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConfidenceActiveLearning",
                table: "Tr_StudySessions",
                type: "decimal(5,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FatigueScore",
                table: "Tr_StudySessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FocusScore",
                table: "Tr_StudySessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdleSeconds",
                table: "Tr_StudySessions",
                type: "int",
                nullable: true);
        }
    }
}
