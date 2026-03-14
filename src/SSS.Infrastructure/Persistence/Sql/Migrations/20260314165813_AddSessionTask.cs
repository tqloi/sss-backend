using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tr_StudySessions_Ct_RoadmapNodes_NodeId",
                table: "Tr_StudySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tr_StudySessions_Pl_StudyPlanModules_ModuleId",
                table: "Tr_StudySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tr_StudySessions_Pl_StudyPlans_StudyPlanId",
                table: "Tr_StudySessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tr_StudySessions_Pl_TaskItems_TaskId",
                table: "Tr_StudySessions");

            migrationBuilder.DropIndex(
                name: "IX_Tr_StudySessions_ModuleId",
                table: "Tr_StudySessions");

            migrationBuilder.DropIndex(
                name: "IX_Tr_StudySessions_NodeId",
                table: "Tr_StudySessions");

            migrationBuilder.DropIndex(
                name: "IX_Tr_StudySessions_StudyPlanId",
                table: "Tr_StudySessions");

            migrationBuilder.DropIndex(
                name: "IX_Tr_StudySessions_TaskId",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "NodeId",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "StudyPlanId",
                table: "Tr_StudySessions");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Tr_StudySessions");

            migrationBuilder.CreateTable(
                name: "Tr_SessionTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudySessionId = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    StartTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tr_SessionTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tr_SessionTasks_Pl_TaskItems_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Pl_TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tr_SessionTasks_Tr_StudySessions_StudySessionId",
                        column: x => x.StudySessionId,
                        principalTable: "Tr_StudySessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_SessionTasks_StudySessionId",
                table: "Tr_SessionTasks",
                column: "StudySessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_SessionTasks_TaskId",
                table: "Tr_SessionTasks",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tr_SessionTasks");

            migrationBuilder.AddColumn<long>(
                name: "ModuleId",
                table: "Tr_StudySessions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NodeId",
                table: "Tr_StudySessions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudyPlanId",
                table: "Tr_StudySessions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TaskId",
                table: "Tr_StudySessions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_ModuleId",
                table: "Tr_StudySessions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_NodeId",
                table: "Tr_StudySessions",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_StudyPlanId",
                table: "Tr_StudySessions",
                column: "StudyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Tr_StudySessions_TaskId",
                table: "Tr_StudySessions",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tr_StudySessions_Ct_RoadmapNodes_NodeId",
                table: "Tr_StudySessions",
                column: "NodeId",
                principalTable: "Ct_RoadmapNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tr_StudySessions_Pl_StudyPlanModules_ModuleId",
                table: "Tr_StudySessions",
                column: "ModuleId",
                principalTable: "Pl_StudyPlanModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tr_StudySessions_Pl_StudyPlans_StudyPlanId",
                table: "Tr_StudySessions",
                column: "StudyPlanId",
                principalTable: "Pl_StudyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tr_StudySessions_Pl_TaskItems_TaskId",
                table: "Tr_StudySessions",
                column: "TaskId",
                principalTable: "Pl_TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
