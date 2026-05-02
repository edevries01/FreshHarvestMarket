using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshHarvestMarket.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Favorites",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Favorites");
        }
    }
}
