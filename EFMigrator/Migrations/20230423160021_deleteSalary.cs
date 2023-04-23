using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFMigrator.Migrations
{
    /// <inheritdoc />
    public partial class deleteSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Families");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Salary",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Balance",
                table: "Families",
                type: "bigint",
                nullable: true);
        }
    }
}
