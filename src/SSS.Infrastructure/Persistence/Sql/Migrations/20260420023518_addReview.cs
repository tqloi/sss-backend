using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class addReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ct_Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoadmapId = table.Column<long>(type: "bigint", nullable: false),
                    ReviewerId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Comment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ct_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ct_Reviews_Ct_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Ct_Roadmaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ct_Reviews_Id_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Id_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_Reviews_ReviewerId",
                table: "Ct_Reviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ct_Reviews_RoadmapId_ReviewerId",
                table: "Ct_Reviews",
                columns: new[] { "RoadmapId", "ReviewerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ct_Reviews");
        }
    }
}
