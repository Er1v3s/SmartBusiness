using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_CreatedBy_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_CreatedById1",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "CreatedById1",
                table: "Companies",
                newName: "CreatedByUserId1");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Companies",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_CreatedById1",
                table: "Companies",
                newName: "IX_Companies_CreatedByUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_CreatedByUserId1",
                table: "Companies",
                column: "CreatedByUserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_CreatedByUserId1",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId1",
                table: "Companies",
                newName: "CreatedById1");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Companies",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_CreatedByUserId1",
                table: "Companies",
                newName: "IX_Companies_CreatedById1");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_CreatedById1",
                table: "Companies",
                column: "CreatedById1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
