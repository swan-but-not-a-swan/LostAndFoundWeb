using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostAndFoundWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddedUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Urls",
                table: "LostItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Urls",
                table: "FoundItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Urls",
                table: "LostItems");

            migrationBuilder.DropColumn(
                name: "Urls",
                table: "FoundItems");
        }
    }
}
