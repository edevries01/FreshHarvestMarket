using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshHarvestMarket.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProduceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_Favorites_Produce_ProduceId",
                        column: x => x.ProduceId,
                        principalTable: "Produce",
                        principalColumn: "ProduceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 1,
                column: "ImageUrl",
                value: "zucchini.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 2,
                column: "ImageUrl",
                value: "tomatoes.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 3,
                column: "ImageUrl",
                value: "honey.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 4,
                column: "ImageUrl",
                value: "plums.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 5,
                column: "ImageUrl",
                value: "potatoes.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 6,
                column: "ImageUrl",
                value: "blueberries.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 7,
                column: "ImageUrl",
                value: "sweetcorn.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 8,
                column: "ImageUrl",
                value: "broccoli.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 9,
                column: "ImageUrl",
                value: "garlic.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 10,
                column: "ImageUrl",
                value: "cherries.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 11,
                column: "ImageUrl",
                value: "carrots.jpg");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 12,
                column: "ImageUrl",
                value: "raspberries.jpg");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ProduceId",
                table: "Favorites",
                column: "ProduceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 1,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 2,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 3,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 4,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 5,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 6,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 7,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 8,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 9,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 10,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 11,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 12,
                column: "ImageUrl",
                value: null);
        }
    }
}
