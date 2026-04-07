using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSS.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class addNotiField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionUrl",
                table: "Nt_UserNotifications",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DedupeKey",
                table: "Nt_UserNotifications",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Nt_UserNotifications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nt_UserNotifications_DedupeKey",
                table: "Nt_UserNotifications",
                column: "DedupeKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nt_UserNotifications_DedupeKey",
                table: "Nt_UserNotifications");

            migrationBuilder.DropColumn(
                name: "ActionUrl",
                table: "Nt_UserNotifications");

            migrationBuilder.DropColumn(
                name: "DedupeKey",
                table: "Nt_UserNotifications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Nt_UserNotifications");
        }
    }
}
