using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFMigrator.Migrations
{
    /// <inheritdoc />
    public partial class fixUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_salary",
                table: "Users",
                newName: "Salary");

            migrationBuilder.RenameColumn(
                name: "_password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "_name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "_email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "_id",
                table: "Users",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Users",
                newName: "_salary");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "_password");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "_name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "_email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "_id");
        }
    }
}
