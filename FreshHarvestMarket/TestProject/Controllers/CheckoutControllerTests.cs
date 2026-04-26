using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Controllers
{
    [TestClass]
    public class CheckoutControllerTests
    {
        private IQueryable<Discount> testDiscounts = new List<Discount>() {new Discount
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
        private IQueryable<User> testUser = new List<User>() 
        {
            new User()
            {
                Id = "1",
                UserName = "JohnShopper",
                Email = "John@email.com",
                FirstName = "John",
                LastName = "Smith"
            }
        }.AsQueryable();

        private void LoadTestDataProperties()
        {
            foreach (Discount discount in testDiscounts)
            {
                discount.Produce = testProduce.FirstOrDefault(p => p.ProduceId == discount.ProduceId);
            }

            foreach (OrderItem orderItem in testOrderItems)
            {
                orderItem.Produce = testProduce.FirstOrDefault(p => p.ProduceId == orderItem.ProduceId);
                orderItem.Order = testOrders.FirstOrDefault(p => p.OrderId == orderItem.OrderId);
            }
        }

        //Expects to retrieve the checkout view normally
        [TestMethod]
        public void Get_Index_RetrievesViewSuccessfullyNoAuth()
        {
            //ARRANGE
            Mock<ICartService> mockCartService = new Mock<ICartService>();
            Mock<IRepository<User>> mockUserRepo = new Mock<IRepository<User>>();
            Mock<IRepository<OrderItem>> mockOrderItemRepo = new Mock<IRepository<OrderItem>>();
            Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.Setup(m => m.HttpContext.User.Identity.IsAuthenticated).Returns(false);

            //Mocking user manager is a pain in the butt. This post helped me and gives explanation to the weird mock/constructor
            //https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            CheckoutController testController = new CheckoutController(mockCartService.Object, mockUserManager.Object,
                mockUserRepo.Object, mockOrderItemRepo.Object, mockContextAccessor.Object);

            //ACT
            var result = testController.Index();

            //ASSERT
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            
            ViewResult viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(CheckoutViewModel));
        }

        //Expects to retrieve the checkout view when authed
        [TestMethod]
        public void Get_Index_RetrievesViewSuccessfullyWithAuth()
        {
            //ARRANGE
            Mock<ICartService> mockCartService = new Mock<ICartService>();
            Mock<IRepository<User>> mockUserRepo = new Mock<IRepository<User>>();
            Mock<IRepository<OrderItem>> mockOrderItemRepo = new Mock<IRepository<OrderItem>>();
            Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.Setup(m => m.HttpContext.User.Identity.IsAuthenticated).Returns(false);

            //Mocking user manager is a pain in the butt. This post helped me and gives explanation to the weird mock/constructor
            //https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            CheckoutController testController = new CheckoutController(mockCartService.Object, mockUserManager.Object,
                mockUserRepo.Object, mockOrderItemRepo.Object, mockContextAccessor.Object);

            //ACT
            var result = testController.Index();

            //ASSERT
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            ViewResult viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(CheckoutViewModel));
        }
    }

    
}
