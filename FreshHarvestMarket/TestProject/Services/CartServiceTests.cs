using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TestProject;

/// <summary>
/// Tests for the CartService class
/// </summary>
[TestClass]
public class CartServiceTests
{
    
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


    List<CartItem> testCartItems = new List<CartItem>()
    {
        new CartItem()
        {
            ProduceId = 1,
            Quantity = 10
        }

    };

    private void LoadTestData()
    {
        foreach (CartItem cartItem in testCartItems)
        {
            cartItem.ProduceId = testProduce.FirstOrDefault(p => p.ProduceId == cartItem.ProduceId)!.ProduceId;
            cartItem.ProduceName = testProduce.FirstOrDefault(p => p.ProduceId == cartItem.ProduceId)!.ProduceName;
            cartItem.ImageUrl = testProduce.FirstOrDefault(p => p.ProduceId == cartItem.ProduceId)!.ImageUrl!;
            cartItem.Price = testProduce.FirstOrDefault(p => p.ProduceId == cartItem.ProduceId)!.UnitPrice;
        }
    }

    /// <summary>
    /// Expects the GetCart method to execute successfully
    /// </summary>
    [TestMethod]
    public void GetCart_Successful()
    {
        //ARRANGE
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        var mockSession = new Mock<ISession>();
        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
        mockContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);

        //ACT
        List<CartItem> result = cartService.GetCart();

        //ASSERT
        Assert.IsTrue(result.Count == 0);
    }

    [TestMethod]
    public void SaveCart_Successful()
    {
        //ARRANGE
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        var mockSession = new Mock<ISession>();
        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
        mockContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);

        //ACT
        cartService.SaveCart(testCartItems);

        //ASSERT
        //Cannot verify SetString because it is extension, so have to verify method underneath the extension one
        mockSession.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);
    }

    [TestMethod]
    public void AddItem_Successfil()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        var mockSession = new Mock<ISession>();
        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
        mockHttpContext.Setup(c => c.User.Identity.IsAuthenticated).Returns(false);
        mockContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);

        //ACT
        cartService.AddItem(testCartItems[0]);

        //ASSERT
        mockProduceRepo.Verify(m => m.GetAll(), Times.Once);

        List<CartItem> cart = cartService.GetCart();
        Assert.IsTrue(cart[0] == testCartItems[0]);
    }
}
