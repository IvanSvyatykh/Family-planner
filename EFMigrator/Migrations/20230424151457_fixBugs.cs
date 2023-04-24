using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFMigrator.Migrations
{
    /// <inheritdoc />
    public partial class fixBugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GoodsCategory",
                table: "GoodsCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Family",
                table: "Family");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "GoodsCategory",
                newName: "Goods");

            migrationBuilder.RenameTable(
                name: "Family",
                newName: "Families");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goods",
                table: "Goods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Families",
                table: "Families",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goods",
                table: "Goods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Families",
                table: "Families");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Goods",
                newName: "GoodsCategory");

            migrationBuilder.RenameTable(
                name: "Families",
                newName: "Family");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoodsCategory",
                table: "GoodsCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Family",
                table: "Family",
                column: "Id");
        }
    }
}
