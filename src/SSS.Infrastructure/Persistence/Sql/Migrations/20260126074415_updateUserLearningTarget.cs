using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateUserLearningTarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Ln_UserLearningTargets",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<long>(
                name: "RoadmapId",
                table: "Ln_UserLearningTargets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Ln_UserLearningTargets_UserId_RoadmapId_Status",
                table: "Ln_UserLearningTargets",
                columns: new[] { "UserId", "RoadmapId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ln_UserLearningTargets_UserId_RoadmapId_Status",
                table: "Ln_UserLearningTargets");

            migrationBuilder.DropColumn(
                name: "RoadmapId",
                table: "Ln_UserLearningTargets");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Ln_UserLearningTargets",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }
    }
}
