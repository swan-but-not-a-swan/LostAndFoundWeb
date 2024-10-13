using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostAndFoundWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFoundItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OwnerFound",
                table: "FoundItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerFound",
                table: "FoundItems");
        }
    }
}
