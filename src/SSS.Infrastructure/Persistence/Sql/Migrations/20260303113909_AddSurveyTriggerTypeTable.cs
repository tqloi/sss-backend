using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyTriggerTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "As_SurveyTriggerTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_As_SurveyTriggerTypes", x => x.Code);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_As_SurveyTriggerMappings_TriggerType",
                table: "As_SurveyTriggerMappings",
                column: "TriggerType");

            migrationBuilder.AddForeignKey(
                name: "FK_As_SurveyTriggerMappings_As_SurveyTriggerTypes_TriggerType",
                table: "As_SurveyTriggerMappings",
                column: "TriggerType",
                principalTable: "As_SurveyTriggerTypes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_As_SurveyTriggerMappings_As_SurveyTriggerTypes_TriggerType",
                table: "As_SurveyTriggerMappings");

            migrationBuilder.DropTable(
                name: "As_SurveyTriggerTypes");

            migrationBuilder.DropIndex(
                name: "IX_As_SurveyTriggerMappings_TriggerType",
                table: "As_SurveyTriggerMappings");
        }
    }
}
