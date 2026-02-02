using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRoadmapVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateById",
                table: "Ct_Roadmaps",
                type: "varchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Ct_Roadmaps",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLatest",
                table: "Ct_Roadmaps",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Ct_Roadmaps",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Ct_Roadmaps_CreateById",
                table: "Ct_Roadmaps",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_Roadmaps_SubjectId_Title_IsLatest",
                table: "Ct_Roadmaps",
                columns: new[] { "SubjectId", "Title", "IsLatest" });

            migrationBuilder.CreateIndex(
                name: "IX_Ct_Roadmaps_SubjectId_Title_Version",
                table: "Ct_Roadmaps",
                columns: new[] { "SubjectId", "Title", "Version" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ct_Roadmaps_Id_Users_CreateById",
                table: "Ct_Roadmaps",
                column: "CreateById",
                principalTable: "Id_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ct_Roadmaps_Id_Users_CreateById",
                table: "Ct_Roadmaps");

            migrationBuilder.DropIndex(
                name: "IX_Ct_Roadmaps_CreateById",
                table: "Ct_Roadmaps");

            migrationBuilder.DropIndex(
                name: "IX_Ct_Roadmaps_SubjectId_Title_IsLatest",
                table: "Ct_Roadmaps");

            migrationBuilder.DropIndex(
                name: "IX_Ct_Roadmaps_SubjectId_Title_Version",
                table: "Ct_Roadmaps");

            migrationBuilder.DropColumn(
                name: "CreateById",
                table: "Ct_Roadmaps");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Ct_Roadmaps");

            migrationBuilder.DropColumn(
                name: "IsLatest",
                table: "Ct_Roadmaps");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Ct_Roadmaps");
        }
    }
}
