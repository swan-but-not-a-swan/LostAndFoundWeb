using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostAndFoundWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpgradedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "LostItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FoundItems");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "LostItems",
                newName: "WordTwo");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "FoundItems",
                newName: "WordTwo");

            migrationBuilder.AddColumn<string>(
                name: "WordOne",
                table: "LostItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordThree",
                table: "LostItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordOne",
                table: "FoundItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordThree",
                table: "FoundItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordOne",
                table: "LostItems");

            migrationBuilder.DropColumn(
                name: "WordThree",
                table: "LostItems");

            migrationBuilder.DropColumn(
                name: "WordOne",
                table: "FoundItems");

            migrationBuilder.DropColumn(
                name: "WordThree",
                table: "FoundItems");

            migrationBuilder.RenameColumn(
                name: "WordTwo",
                table: "LostItems",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "WordTwo",
                table: "FoundItems",
                newName: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LostItems",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FoundItems",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");
        }
    }
}
