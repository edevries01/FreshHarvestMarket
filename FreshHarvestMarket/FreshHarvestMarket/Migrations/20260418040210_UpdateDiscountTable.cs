using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FreshHarvestMarket.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Discounts_ProduceId",
                table: "Discounts");

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "DiscountId", "DiscountAmount", "ProduceId" },
                values: new object[,]
                {
                    { 1, 50, 1 },
                    { 2, 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ProduceId",
                table: "Discounts",
                column: "ProduceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Discounts_ProduceId",
                table: "Discounts");

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ProduceId",
                table: "Discounts",
                column: "ProduceId");
        }
    }
}
