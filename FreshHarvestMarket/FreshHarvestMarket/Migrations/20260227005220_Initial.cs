using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FreshHarvestMarket.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produce",
                columns: table => new
                {
                    ProduceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProduceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProduceDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProduceCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produce", x => x.ProduceId);
                });

            migrationBuilder.InsertData(
                table: "Produce",
                columns: new[] { "ProduceId", "InventoryTotal", "ProduceCategory", "ProduceDescription", "ProduceName", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 20, "Vegetable", "Fresh green zucchinis", "Zuccihini", 1.20m },
                    { 2, 25, "Vegetable", "Ripe red tomatoes", "Tomatoes", 2.00m },
                    { 3, 15, "Other", "Local raw honey", "Honey", 6.00m },
                    { 4, 18, "Fruit", "Juicy plums", "Plums", 2.50m },
                    { 5, 30, "Vegetable", "Freshly harvested potatoes", "Potatoes", 1.20m },
                    { 6, 12, "Fruit", "Sweet blueberries", "Blueberries", 3.00m },
                    { 7, 20, "Vegetable", "Fresh sweet corn", "Sweet Corn", 1.75m },
                    { 8, 15, "Vegetable", "Green broccoli florets", "Broccoli", 2.25m },
                    { 9, 40, "Vegetable", "Fresh garlic bulbs", "Garlic", 0.80m },
                    { 10, 10, "Fruit", "Sweet red cherries", "Cherries", 3.50m },
                    { 11, 25, "Vegetable", "Organic carrots", "Carrots", 1.50m },
                    { 12, 12, "Fruit", "Fresh raspberries", "Raspberries", 3.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produce");
        }
    }
}
