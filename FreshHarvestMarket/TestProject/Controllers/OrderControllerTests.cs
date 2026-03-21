namespace TestProject;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using FreshHarvestMarket.Controllers;
using Microsoft.AspNetCore.Mvc;

[TestClass]
public class OrderControllerTests
{
    private FreshMarketContext GetInMemoryContext()
    {
        //Make new in-memory database
        DbContextOptions options = new DbContextOptionsBuilder<FreshMarketContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        var context = new FreshMarketContext(options);

        //Add data and save
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
                OrderTotal = 12.50m,
                OrderDate = new DateTime(2026, 3, 1),
                PickupDate = new DateTime(2026, 3, 2),
                IsPickedUp = false
            },
            new Order()
            {
                OrderId = 2,
                OrderTotal = 22.00m,
                OrderDate = new DateTime(2026, 3, 3),
                PickupDate = new DateTime(2026, 3, 4),
                IsPickedUp = false
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

    [TestMethod]
    public void OrderController_ManageOrders()
    {
        //Arrange
        FreshMarketContext ctx = GetInMemoryContext();
        OrderController controller = new OrderController( ctx );

        //Act
        ViewResult result = controller.ManageOrders();

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType( result.Model, typeof(List<Order>));

        List<Order> orders = (List<Order>)result.Model;

        //All should be active orders
        Assert.IsFalse(orders.Any(o => o.IsPickedUp == true));

        Order firstOrder = orders[0];
        Order lastOrder = orders[orders.Count - 1];
        Assert.IsTrue(firstOrder.OrderDate <  lastOrder.OrderDate);
    }
}
