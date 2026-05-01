/// <summary>
/// Unit tests for the Order model.
/// 
/// Verifies order-related logic such as calculating order totals using
/// in-memory database data. This allows testing of Order behavior,
/// including relationships with OrderItems & Produce, without
/// requiring a real database connection.
/// </summary>

using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using Microsoft.EntityFrameworkCore;
using FreshHarvestMarket.Data;
using Microsoft.Testing.Platform.MSBuild;

namespace TestProject;

[TestClass]
public class OrderTests
{
    /// <summary>
    /// Returns an in-memory database
    /// 
    /// Makes it easier to write new test cases
    /// </summary>
    /// <returns>In-memory database</returns>
    private FreshHarvestContext GetInMemoryContext()
    {
        //Make new in-memory database
        DbContextOptions options = new DbContextOptionsBuilder<FreshHarvestContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        var context = new FreshHarvestContext(options);

        //Add data and save
        //This was last updated 3/23/2026 and is based on the current context class data
        context.Produce.AddRange(
            new Produce()
            {
                ProduceId = 1,
                ProduceName = "Zuccihini",
                ProduceDescription = "Fresh green zucchinis",
                UnitPrice = 1.20m,
                ProduceCategory = "Vegetable",
                InventoryTotal = 20
            },
            new Produce()
            {
                ProduceId = 2,
                ProduceName = "Tomatoes",
                ProduceDescription = "Ripe red tomatoes",
                UnitPrice = 2.00m,
                ProduceCategory = "Vegetable",
                InventoryTotal = 25
            },
            new Produce()
            {
                ProduceId = 3,
                ProduceName = "Honey",
                ProduceDescription = "Local raw honey",
                UnitPrice = 6.00m,
                ProduceCategory = "Other",
                InventoryTotal = 15
            }
        );

        context.Orders.AddRange(
            new Order()
            {
                OrderId = 1,
                OrderTotal = 0m,
                OrderDate = new DateTime(2026, 3, 1),
                PickupDate = new DateTime(2026, 3, 2),
                IsPickedUp = false,
                Rejected = false
            },
            new Order()
            {
                OrderId = 2,
                OrderTotal = 22.00m,
                OrderDate = new DateTime(2026, 3, 3),
                PickupDate = new DateTime(2026, 3, 4),
                IsPickedUp = false,
                Rejected = false
            }
        );

        context.OrderItems.AddRange(
            new OrderItem
            {
                OrderItemId = 1,
                OrderId = 1,
                ProduceId = 1,
                Quantity = 3
            },
            new OrderItem
            {
                OrderItemId = 2,
                OrderId = 1,
                ProduceId = 2,
                Quantity = 2
            },
            new OrderItem
            {
                OrderItemId = 3,
                OrderId = 2,
                ProduceId = 3,
                Quantity = 4
            }
        );

        context.SaveChanges();
        return context;
    }

    /// <summary>
    /// Tests the UpdateOrderTotal method of the Order class
    /// </summary>
    [TestMethod]
    public void Order_UpdateOrderTotal() 
    {
        //Arrange
        FreshHarvestContext context = GetInMemoryContext();

        //Act
        Order testOrder = context.Orders.Include(o => o.Items).FirstOrDefault()!;

        //Assert
        Assert.IsTrue(testOrder.OrderTotal == 0);
        testOrder.UpdateOrderTotal();
        Assert.IsTrue(testOrder.OrderTotal == 7.60m);
    }
}
