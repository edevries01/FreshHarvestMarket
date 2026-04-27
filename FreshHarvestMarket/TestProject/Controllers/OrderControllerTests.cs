namespace TestProject;
using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.OtherServices;
using FreshHarvestMarket.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using System;

[TestClass]
public class OrderControllerTests
{
    //Fake produce for testing
    //Can set repo mocks to return these collections for predictable data
    private IQueryable<Produce> testProduce = new List<Produce>()
    {
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
    }.AsQueryable();

    private IQueryable<Order> testOrders = new List<Order>()
    {
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
    }.AsQueryable();

    private IQueryable<OrderItem> testOrderItems = new List<OrderItem>()
    {
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
    }.AsQueryable();

    private void LoadProperties()
    {
        foreach (OrderItem orderItem in testOrderItems)
        {
            orderItem.Produce = testProduce.FirstOrDefault(p => p.ProduceId == orderItem.ProduceId);
            orderItem.Order = testOrders.FirstOrDefault(p => p.OrderId == orderItem.OrderId);
        }
    }

    /// <summary>
    /// Scenario that expects success
    /// when calling 'GET' on the 'Order' controller with the 'ManageOrders' action
    /// </summary>
    [TestMethod]
    public void OrderController_ManageOrders()
    {
        //Arrange
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object,mockProduceRepo.Object, mockUserManager.Object);

        //Act
        var result = controller.ManageOrders();

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        ViewResult resultViewResult = (ViewResult)result;
        Assert.IsInstanceOfType( resultViewResult.Model, typeof(List<Order>));

        List<Order> orders = (List<Order>)resultViewResult.Model;

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
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object, mockProduceRepo.Object, mockUserManager.Object);

        //Act
        var result = controller.ViewOrder(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        ViewResult resultViewResult = (ViewResult)result;
        Assert.IsInstanceOfType(resultViewResult.Model, typeof(Order));

        Order order = (Order)resultViewResult.Model;

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
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object, mockProduceRepo.Object, mockUserManager.Object);

        //Act
        var result = controller.ConfirmReject(1);

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
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object, mockProduceRepo.Object, mockUserManager.Object);

        //Act
        var result = controller.ConfirmReject(99999);

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
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object, mockProduceRepo.Object, mockUserManager.Object);
        Order testOrder = testOrders.FirstOrDefault(o => o.OrderId == 1)!;

        //Act
        var result = controller.UpdateRejected(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Verify it was updated
        //mockDiscountRepo.Verify(r => r.GetAll(), Times.Once);
        mockOrderRepo.Verify(r => r.Update(testOrder), Times.Once());
        mockOrderRepo.Verify(r => r.Save(), Times.Once());
    }

    /// <summary>
    /// Scenario that expects NotFound scenario
    /// when calling 'POST' on the 'Order' controller's UpdateRejected action
    /// </summary>
    [TestMethod]
    public void OrderController_UpdateRejected_NotFound()
    {
        //Arrange
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object, mockProduceRepo.Object, mockUserManager.Object);

        //Act
        var result = controller.UpdateRejected(99999);

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
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object, mockProduceRepo.Object, mockUserManager.Object);

        Order testOrder = testOrders.FirstOrDefault(o => o.OrderId == 1)!;

        //Act
        var result = controller.UpdateFulfilled(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Order? updatedOrder = context.Orders.Find(1);
        //Assert.IsNotNull(updatedOrder);
        //Assert.IsTrue(updatedOrder.IsPickedUp);
        mockOrderRepo.Verify(r => r.Update(testOrder), Times.Once());
        mockOrderRepo.Verify(r => r.Save(), Times.Once());
    }

    /// <summary>
    /// Scenario that expects NotFound scenario
    /// when calling 'POST' on the 'Order' controller's UpdateFufilled action
    /// </summary>
    [TestMethod]
    public void OrderController_UpdateFufilled_NotFound()
    {
        //Arrange
        LoadProperties();
        Mock<IRepository<Order>> mockOrderRepo = new Mock<IRepository<Order>>();
        mockOrderRepo.Setup(r => r.GetAll()).Returns(testOrders);
        Mock<IOrderFiltersSession> mockOrderFilterSession = new Mock<IOrderFiltersSession>();
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();

        //Mocking user manager is a pain in the butt. This post helped me and gives explanation to the weird mock/constructor
        //https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        OrderController controller = new OrderController(mockOrderRepo.Object, mockOrderFilterSession.Object, mockProduceRepo.Object, mockUserManager.Object);

        //Act
        var result = controller.UpdateFulfilled(99999);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
}
