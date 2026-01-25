using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Ln_UserLearningBehaviors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Ln_UserLearningTargets",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)");

            migrationBuilder.AddColumn<DateTime>(
                name: "SnapshotAt",
                table: "Ln_UserLearningTargets",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "SourceSurveyResponseId",
                table: "Ln_UserLearningTargets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "SnapshotAt",
                table: "Ln_UserLearningBehaviors",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SnapshotVersion",
                table: "Ln_UserLearningBehaviors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "SourceSurveyResponseId",
                table: "Ln_UserLearningBehaviors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnapshotAt",
                table: "Ln_UserLearningTargets");

            migrationBuilder.DropColumn(
                name: "SourceSurveyResponseId",
                table: "Ln_UserLearningTargets");

            migrationBuilder.DropColumn(
                name: "SnapshotAt",
                table: "Ln_UserLearningBehaviors");

            migrationBuilder.DropColumn(
                name: "SnapshotVersion",
                table: "Ln_UserLearningBehaviors");

            migrationBuilder.DropColumn(
                name: "SourceSurveyResponseId",
                table: "Ln_UserLearningBehaviors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Ln_UserLearningTargets",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Ln_UserLearningBehaviors",
                type: "datetime(6)",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP(6)");
        }
    }
}
