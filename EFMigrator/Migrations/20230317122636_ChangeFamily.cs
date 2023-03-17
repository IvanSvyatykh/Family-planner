using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFMigrator.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFamily : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Users_UserId",
                table: "Families");

            migrationBuilder.DropIndex(
                name: "IX_Families_UserId",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Families");

            migrationBuilder.AddColumn<long>(
                name: "FamilyId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatorId",
                table: "Families",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Families");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Families",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_UserId",
                table: "Families",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Users_UserId",
                table: "Families",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
