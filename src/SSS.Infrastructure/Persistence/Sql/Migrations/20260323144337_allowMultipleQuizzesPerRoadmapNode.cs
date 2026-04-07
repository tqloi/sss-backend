using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class allowMultipleQuizzesPerRoadmapNode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_As_Quizzes_Ct_RoadmapNodes_RoadmapNodeId",
                table: "As_Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_As_Quizzes_RoadmapNodeId",
                table: "As_Quizzes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PassingScore",
                table: "As_Quizzes",
                type: "decimal(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "As_Quizzes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "IX_As_Quizzes_RoadmapNodeId",
                table: "As_Quizzes",
                column: "RoadmapNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_As_Quizzes_Ct_RoadmapNodes_RoadmapNodeId",
                table: "As_Quizzes",
                column: "RoadmapNodeId",
                principalTable: "Ct_RoadmapNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_As_Quizzes_Ct_RoadmapNodes_RoadmapNodeId",
                table: "As_Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_As_Quizzes_RoadmapNodeId",
                table: "As_Quizzes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PassingScore",
                table: "As_Quizzes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "As_Quizzes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_As_Quizzes_RoadmapNodeId",
                table: "As_Quizzes",
                column: "RoadmapNodeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_As_Quizzes_Ct_RoadmapNodes_RoadmapNodeId",
                table: "As_Quizzes",
                column: "RoadmapNodeId",
                principalTable: "Ct_RoadmapNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
