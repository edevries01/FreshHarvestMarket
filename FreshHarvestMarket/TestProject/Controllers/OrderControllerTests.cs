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
    /// <summary>
    /// Returns an in-memory database for testing
    /// </summary>
    /// <returns>An in-memory database</returns>
    private FreshMarketContext GetInMemoryContext()
    {
        //Make new in-memory database
        DbContextOptions options = new DbContextOptionsBuilder<FreshMarketContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        var context = new FreshMarketContext(options);

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
                OrderTotal = 12.50m,
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
    /// Scenario that expects success
    /// when calling 'GET' on the 'Order' controller with the 'ManageOrders' action
    /// </summary>
    [TestMethod]
    public void OrderController_ManageOrders()
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

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

    /// <summary>
    /// Scenario that expects success
    /// when calling 'GET' on the 'Order' controller's ViewOrder action
    /// </summary>
    [TestMethod]
    public void OrderController_ViewOrder() 
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

        //Act
        ViewResult result = controller.ViewOrder(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Model, typeof(Order));

        Order order = (Order)result.Model;

        //Verify correct order was grabbed
        Assert.IsTrue(order.OrderId == 1);
        Assert.IsTrue(order.OrderTotal == 12.50m);
    }

    /// <summary>
    /// Scenario that expects success
    /// when calling 'GET' on the 'Order' controller's ConfirmReject action
    /// </summary>
    [TestMethod]
    public void OrderController_ConfirmReject()
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

        //Act
        ActionResult result = controller.ConfirmReject(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));

        ViewResult viewResult = (ViewResult)result;
        Assert.IsInstanceOfType(viewResult.Model, typeof(Order));

        Order order = (Order)viewResult.Model;

        //Verify correct order was grabbed
        Assert.IsTrue(order.OrderId == 1);
        Assert.IsTrue(order.OrderTotal == 12.50m);
    }

    /// <summary>
    /// Scenario that expects a not found scenario
    /// when calling 'GET' on the 'Order' controller's ConfirmReject action
    /// </summary>
    [TestMethod]
    public void OrderController_ConfirmReject_NotFound()
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

        //Act
        ActionResult result = controller.ConfirmReject(99999);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    /// <summary>
    /// Scenario that expects success
    /// when calling 'POST' on the 'Order' controller's UpdateRejected action
    /// </summary>
    [TestMethod]
    public void OrderController_UpdateRejected() 
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

        //Act
        ActionResult result = controller.UpdateRejected(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Verify it was updated
        Order? updatedOrder = context.Orders.Find(1);
        Assert.IsNotNull(updatedOrder);
        Assert.IsTrue(updatedOrder.Rejected);
    }

    /// <summary>
    /// Scenario that expects NotFound scenario
    /// when calling 'POST' on the 'Order' controller's UpdateRejected action
    /// </summary>
    [TestMethod]
    public void OrderController_UpdateRejected_NotFound()
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

        //Act
        ActionResult result = controller.UpdateRejected(99999);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    /// <summary>
    /// Scenario that expects success
    /// when calling 'POST' on the 'Order' controller's UpdateFufilled action
    /// </summary>
    [TestMethod]
    public void OrderController_UpdateFufilled()
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

        //Act
        ActionResult result = controller.UpdateFufilled(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        Order? updatedOrder = context.Orders.Find(1);
        Assert.IsNotNull(updatedOrder);
        Assert.IsTrue(updatedOrder.IsPickedUp);
    }

    /// <summary>
    /// Scenario that expects NotFound scenario
    /// when calling 'POST' on the 'Order' controller's UpdateFufilled action
    /// </summary>
    [TestMethod]
    public void OrderController_UpdateFufilled_NotFound()
    {
        //Arrange
        FreshMarketContext context = GetInMemoryContext();
        OrderController controller = new OrderController(context);

        //Act
        ActionResult result = controller.UpdateFufilled(99999);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
}
