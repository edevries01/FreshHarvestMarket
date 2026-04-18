using FreshHarvestMarket.Models;
using System.Net.Sockets;

namespace TestProject;

[TestClass]
public class DiscountControllerTests
{
    //Copied the seed data from FreshHarvestContext
    IQueryable<Produce> testProduce = new List<Produce> {new Produce()
                {
                    ProduceId = 1,
                    ProduceName = "Zucchini",
                    ProduceDescription = "Fresh green zucchini from Williamsburg, Iowa",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 20,
                    ImageUrl = "zucchini.jpg"
                },
                new Produce()
                {
                    ProduceId = 2,
                    ProduceName = "Tomatoes",
                    ProduceDescription = "Ripe red tomatoes from Victor, Iowa",
                    UnitPrice = 2.00m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25,
                    ImageUrl = "tomatoes.jpg"
                },
                new Produce()
                {
                    ProduceId = 3,
                    ProduceName = "Honey",
                    ProduceDescription = "Local raw honey from Victor, Iowa",
                    UnitPrice = 6.00m,
                    ProduceCategory = "Other",
                    InventoryTotal = 15,
                    ImageUrl = "honey.jpg"
                },
                new Produce()
                {
                    ProduceId = 4,
                    ProduceName = "Plums",
                    ProduceDescription = "Juicy plums from Amana, Iowa",
                    UnitPrice = 2.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 18,
                    ImageUrl = "plums.jpg"
                },
                new Produce()
                {
                    ProduceId = 5,
                    ProduceName = "Potatoes",
                    ProduceDescription = "Freshly harvested potatoes from Ladora, Iowa",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 30,
                    ImageUrl = "potatoes.jpg"
                },
                new Produce()
                {
                    ProduceId = 6,
                    ProduceName = "Blueberries",
                    ProduceDescription = "Sweet blueberries from Tiffin, Iowa",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12,
                    ImageUrl = "blueberries.jpg"
                },
                new Produce()
                {
                    ProduceId = 7,
                    ProduceName = "Sweet Corn",
                    ProduceDescription = "Fresh sweet corn from Marengo, Iowa",
                    UnitPrice = 1.75m,
                    InventoryTotal = 20,
                    ImageUrl = "sweetcorn.jpg"
                },
                new Produce()
                {
                    ProduceId = 8,
                    ProduceName = "Broccoli",
                    ProduceDescription = "Green broccoli florets from Millersburg, Iowa",
                    UnitPrice = 2.25m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 15,
                    ImageUrl = "broccoli.jpg"
                },
                new Produce()
                {
                    ProduceId = 9,
                    ProduceName = "Garlic",
                    ProduceDescription = "Fresh garlic bulbs from Montezuma, Iowa",
                    UnitPrice = 0.80m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 40,
                    ImageUrl = "garlic.jpg"
                },
                new Produce()
                {
                    ProduceId = 10,
                    ProduceName = "Cherries",
                    ProduceDescription = "Sweet red cherries from Solon, Iowa",
                    UnitPrice = 3.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 10,
                    ImageUrl = "cherries.jpg"
                },
                new Produce()
                {
                    ProduceId = 11,
                    ProduceName = "Carrots",
                    ProduceDescription = "Organic carrots from Kalona, Iowa",
                    UnitPrice = 1.50m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25,
                    ImageUrl = "carrots.jpg"
                },
                new Produce()
                {
                    ProduceId = 12,
                    ProduceName = "Raspberries",
                    ProduceDescription = "Fresh raspberries from Swisher, Iowa",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12,
                    ImageUrl = "raspberries.jpg"
                } }.AsQueryable();

    IQueryable<Discount> testDiscounts = new List<Discount>() {new Discount
                {
                    DiscountId = 1,
                    DiscountAmount = 50,
                    ProduceId = 1
                },
                new Discount
                {
                    DiscountId = 2,
                    DiscountAmount = 3,
                    ProduceId = 3
                } }.AsQueryable();

    //Had to just assign everything manually since .Load() leads to nullreference exception on IQueryable
    private void LoadProperties()
    {
        foreach (Discount discount in testDiscounts) 
        {
            discount.Produce = testProduce.FirstOrDefault(p => p.ProduceId == discount.ProduceId);
        }
    }

    [TestMethod]
    public void TestMethod1()
    {
    }
}
