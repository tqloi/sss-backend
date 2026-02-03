using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Ct_Roadmaps",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "As_SurveyFieldSemantics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SurveyQuestionId = table.Column<long>(type: "bigint", nullable: false),
                    DimensionCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Evaluates = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    AIHint = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    Weight = table.Column<double>(type: "double", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_SurveyFieldSemantics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_SurveyFieldSemantics_As_SurveyQuestions_SurveyQuestionId",
                        column: x => x.SurveyQuestionId,
                        principalTable: "As_SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "As_SurveyTriggerMappings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SurveyId = table.Column<long>(type: "bigint", nullable: false),
                    TriggerType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MaxAttempts = table.Column<int>(type: "int", nullable: true),
                    CooldownDays = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_SurveyTriggerMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_As_SurveyTriggerMappings_As_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "As_Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyFieldSemantics_SurveyQuestionId",
                table: "As_SurveyFieldSemantics",
                column: "SurveyQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyTriggerMappings_SurveyId_TriggerType",
                table: "As_SurveyTriggerMappings",
                columns: new[] { "SurveyId", "TriggerType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "As_SurveyFieldSemantics");

            migrationBuilder.DropTable(
                name: "As_SurveyTriggerMappings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Ct_Roadmaps");
        }
    }
}
