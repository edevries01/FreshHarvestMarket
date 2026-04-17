using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshHarvestMarket.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountAmount = table.Column<int>(type: "int", nullable: false),
                    ProduceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_Discounts_Produce_ProduceId",
                        column: x => x.ProduceId,
                        principalTable: "Produce",
                        principalColumn: "ProduceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 1,
                columns: new[] { "ProduceDescription", "ProduceName" },
                values: new object[] { "Fresh green zucchini from Williamsburg, Iowa", "Zucchini" });

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 2,
                column: "ProduceDescription",
                value: "Ripe red tomatoes from Victor, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 3,
                column: "ProduceDescription",
                value: "Local raw honey from Victor, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 4,
                column: "ProduceDescription",
                value: "Juicy plums from Amana, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 5,
                column: "ProduceDescription",
                value: "Freshly harvested potatoes from Ladora, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 6,
                column: "ProduceDescription",
                value: "Sweet blueberries from Tiffin, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 7,
                columns: new[] { "ProduceCategory", "ProduceDescription" },
                values: new object[] { "", "Fresh sweet corn from Marengo, Iowa" });

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 8,
                column: "ProduceDescription",
                value: "Green broccoli florets from Millersburg, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 9,
                column: "ProduceDescription",
                value: "Fresh garlic bulbs from Montezuma, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 10,
                column: "ProduceDescription",
                value: "Sweet red cherries from Solon, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 11,
                column: "ProduceDescription",
                value: "Organic carrots from Kalona, Iowa");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 12,
                column: "ProduceDescription",
                value: "Fresh raspberries from Swisher, Iowa");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ProduceId",
                table: "Discounts",
                column: "ProduceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 1,
                columns: new[] { "ProduceDescription", "ProduceName" },
                values: new object[] { "Fresh green zucchinis", "Zuccihini" });

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 2,
                column: "ProduceDescription",
                value: "Ripe red tomatoes");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 3,
                column: "ProduceDescription",
                value: "Local raw honey");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 4,
                column: "ProduceDescription",
                value: "Juicy plums");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 5,
                column: "ProduceDescription",
                value: "Freshly harvested potatoes");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 6,
                column: "ProduceDescription",
                value: "Sweet blueberries");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 7,
                columns: new[] { "ProduceCategory", "ProduceDescription" },
                values: new object[] { "Vegetable", "Fresh sweet corn" });

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 8,
                column: "ProduceDescription",
                value: "Green broccoli florets");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 9,
                column: "ProduceDescription",
                value: "Fresh garlic bulbs");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 10,
                column: "ProduceDescription",
                value: "Sweet red cherries");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 11,
                column: "ProduceDescription",
                value: "Organic carrots");

            migrationBuilder.UpdateData(
                table: "Produce",
                keyColumn: "ProduceId",
                keyValue: 12,
                column: "ProduceDescription",
                value: "Fresh raspberries");
        }
    }
}
