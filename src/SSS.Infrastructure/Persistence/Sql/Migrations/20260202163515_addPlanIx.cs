using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPlanIx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pl_StudyPlans_UserId_RoadmapId",
                table: "Pl_StudyPlans",
                columns: new[] { "UserId", "RoadmapId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pl_StudyPlans_UserId",
                table: "Pl_StudyPlans",
                column: "UserId");
        }
    }
}
