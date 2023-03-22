using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFMigrator.Migrations
{
    /// <inheritdoc />
    public partial class ChageCreatorIdToEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Families");

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                table: "Families",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "_creatorEmail",
                table: "Families",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_creatorEmail",
                table: "Families");

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                table: "Families",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatorId",
                table: "Families",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
