using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SSS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addContentManagerSubjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ct_ContentManagerSubject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ContentManagerId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    AssignedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_ContentManagerSubject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ct_ContentManagerSubject_Ct_LearningSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Ct_LearningSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ct_ContentManagerSubject_Id_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Ct_ContentManagerSubject_Id_Users_ContentManagerId",
                        column: x => x.ContentManagerId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_ContentManagerSubject_AssignedBy",
                table: "Ct_ContentManagerSubject",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_ContentManagerSubject_ContentManagerId",
                table: "Ct_ContentManagerSubject",
                column: "ContentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_ContentManagerSubject_ContentManagerId_SubjectId",
                table: "Ct_ContentManagerSubject",
                columns: new[] { "ContentManagerId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ct_ContentManagerSubject_SubjectId",
                table: "Ct_ContentManagerSubject",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ct_ContentManagerSubject");
        }
    }
}
