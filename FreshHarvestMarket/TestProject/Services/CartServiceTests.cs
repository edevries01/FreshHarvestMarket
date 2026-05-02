using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
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
        },

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

    /// <summary>
    /// Checks if we successfully add an item to a cart
    /// </summary>
    [TestMethod]
    public void AddItem_Successfil()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        DefaultHttpContext context = new DefaultHttpContext();
        mockContextAccessor.Setup(m => m.HttpContext)
            .Returns(context);
        mockContextAccessor.Setup(m => m.HttpContext!.Request)
            .Returns(context.Request);
        mockContextAccessor.Setup(m => m.HttpContext!.Response)
            .Returns(context.Response);
        mockContextAccessor.Setup(m => m.HttpContext!.Request.Cookies)
            .Returns(context.Request.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.Response.Cookies)
            .Returns(context.Response.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(false);


        Mock<ISession> mockSession = new Mock<ISession>();
        mockContextAccessor.Setup(m => m.HttpContext!.Session)
                .Returns(mockSession.Object);
        string fakeJson = JsonSerializer.Serialize(testCartItems);
        byte[] fakeBytes = Encoding.UTF8.GetBytes(fakeJson);
        //Cannot mock extension methods... but with Moq we can mock the method under the extension method
        mockSession.Setup(s => s.TryGetValue("CART", out fakeBytes)).Returns(true);


        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);

        //ACT
        cartService.AddItem(testCartItems[0]);

        //ASSERT
        mockProduceRepo.Verify(m => m.GetAll(), Times.Once);

        List<CartItem> cart = cartService.GetCart();
        Assert.AreEqual(cart[0].ProduceId, testCartItems[0].ProduceId);
        Assert.AreEqual(cart[0].Quantity, testCartItems[0].Quantity);
        mockDiscountRepo.Verify(m => m.GetAll(), Times.Never);
    }

    /// <summary>
    /// Checks to see if we apply the discount when the user is authenticated
    /// </summary>
    [TestMethod]
    public void AddItem_ApplyDiscountWhenAuthenticated()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        DefaultHttpContext context = new DefaultHttpContext();
        mockContextAccessor.Setup(m => m.HttpContext)
            .Returns(context);
        mockContextAccessor.Setup(m => m.HttpContext!.Request)
            .Returns(context.Request);
        mockContextAccessor.Setup(m => m.HttpContext!.Response)
            .Returns(context.Response);
        mockContextAccessor.Setup(m => m.HttpContext!.Request.Cookies)
            .Returns(context.Request.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.Response.Cookies)
            .Returns(context.Response.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(true);


        Mock<ISession> mockSession = new Mock<ISession>();
        mockContextAccessor.Setup(m => m.HttpContext!.Session)
                .Returns(mockSession.Object);
        string fakeJson = JsonSerializer.Serialize(testCartItems);
        byte[] fakeBytes = Encoding.UTF8.GetBytes(fakeJson);
        //Cannot mock extension methods... but with Moq we can mock the method under the extension method
        mockSession.Setup(s => s.TryGetValue("CART", out fakeBytes)).Returns(true);


        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);

        //ACT
        cartService.AddItem(testCartItems[0]);

        //ASSERT
        //Should grab discounts if we were applying them
        mockDiscountRepo.Verify(m => m.GetAll(), Times.Once);
    }

    [TestMethod]
    public void RemoveItem_Successfully()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        DefaultHttpContext context = new DefaultHttpContext();
        mockContextAccessor.Setup(m => m.HttpContext)
            .Returns(context);
        mockContextAccessor.Setup(m => m.HttpContext!.Request)
            .Returns(context.Request);
        mockContextAccessor.Setup(m => m.HttpContext!.Response)
            .Returns(context.Response);
        mockContextAccessor.Setup(m => m.HttpContext!.Request.Cookies)
            .Returns(context.Request.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.Response.Cookies)
            .Returns(context.Response.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(true);

        Mock<ISession> mockSession = new Mock<ISession>();

        //I was having trouble figuring out how to handle testing around ISession
        //The Murach textbook talks about this time, but in a limited capacity
        //I asked Claude how people can handle mimicing Get/Set with byte data, and it showed me code simialar to this
        //I am implementing it here, it should make the mock store/retrieve like a dictionary
        Dictionary<string, byte[]> sessionStorage = new Dictionary<string, byte[]>();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
        .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

        mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                var found = sessionStorage.TryGetValue(key, out value!);
                return found;
            });
        mockContextAccessor.Setup(m => m.HttpContext!.Session)
                .Returns(mockSession.Object);

        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);
        cartService.AddItem(testCartItems[0]);
        //ACT
        cartService.RemoveItem(1);

        //ASSERT
        List<CartItem> cart = cartService.GetCart();
        Assert.IsTrue(cart.Count == 0);
    }

    [TestMethod]
    public void GetTotal_Successfully()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        DefaultHttpContext context = new DefaultHttpContext();
        mockContextAccessor.Setup(m => m.HttpContext)
            .Returns(context);
        mockContextAccessor.Setup(m => m.HttpContext!.Request)
            .Returns(context.Request);
        mockContextAccessor.Setup(m => m.HttpContext!.Response)
            .Returns(context.Response);
        mockContextAccessor.Setup(m => m.HttpContext!.Request.Cookies)
            .Returns(context.Request.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.Response.Cookies)
            .Returns(context.Response.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(true);

        Mock<ISession> mockSession = new Mock<ISession>();

        //I was having trouble figuring out how to handle testing around ISession
        //The Murach textbook talks about this time, but in a limited capacity
        //I asked Claude how people can handle mimicing Get/Set with byte data, and it showed me code similar to this
        //I am implementing it here, it should make the mock store/ retrieve like a dictionary
        Dictionary<string, byte[]> sessionStorage = new Dictionary<string, byte[]>();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>())).Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

        mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                var found = sessionStorage.TryGetValue(key, out value!);
                return found;
            });
        mockContextAccessor.Setup(m => m.HttpContext!.Session)
                .Returns(mockSession.Object);


        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);
        cartService.AddItem(testCartItems[0]);
        //ACT
        decimal total = cartService.GetTotal();

        //ASSERT
        Assert.IsTrue(total == 12);
    }

    [TestMethod]
    public void ClearCart_Successfully()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        DefaultHttpContext context = new DefaultHttpContext();
        mockContextAccessor.Setup(m => m.HttpContext)
            .Returns(context);
        mockContextAccessor.Setup(m => m.HttpContext!.Request)
            .Returns(context.Request);
        mockContextAccessor.Setup(m => m.HttpContext!.Response)
            .Returns(context.Response);
        mockContextAccessor.Setup(m => m.HttpContext!.Request.Cookies)
            .Returns(context.Request.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.Response.Cookies)
            .Returns(context.Response.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(true);

        Mock<ISession> mockSession = new Mock<ISession>();
        mockContextAccessor.Setup(m => m.HttpContext!.Session)
                .Returns(mockSession.Object);


        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);
        
        //ACT
        cartService.ClearCart();

        //ASSERT
        mockSession.Verify(m => m.Remove("CART"), Times.Once());
    }

    [TestMethod]
    public void UpdateQuantity_Successfully()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        DefaultHttpContext context = new DefaultHttpContext();
        mockContextAccessor.Setup(m => m.HttpContext)
            .Returns(context);
        mockContextAccessor.Setup(m => m.HttpContext!.Request)
            .Returns(context.Request);
        mockContextAccessor.Setup(m => m.HttpContext!.Response)
            .Returns(context.Response);
        mockContextAccessor.Setup(m => m.HttpContext!.Request.Cookies)
            .Returns(context.Request.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.Response.Cookies)
            .Returns(context.Response.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(true);

        Mock<ISession> mockSession = new Mock<ISession>();

        //I was having trouble figuring out how to handle testing around ISession
        //The Murach textbook talks about this time, but in a limited capacity
        //I asked Claude how people can handle mimicing Get/Set with byte data, and it showed me code simialar to this
        //I am implementing it here, it should make the mock store/retrieve like a dictionary
        Dictionary<string, byte[]> sessionStorage = new Dictionary<string, byte[]>();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
        .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

        mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                var found = sessionStorage.TryGetValue(key, out value!);
                return found;
            });
        mockContextAccessor.Setup(m => m.HttpContext!.Session)
                .Returns(mockSession.Object);

        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);
        //Add single item to cart
        cartService.AddItem(testCartItems[0]);

        //ACT
        cartService.UpdateQuantity(1, 100); //More than what is in inventory

        //ASSERT
        List<CartItem> cart = cartService.GetCart();
        //Since it is more than what is in inventory, it should default to the max left
        Assert.IsTrue(cart[0].Quantity == 20);
    }

    [TestMethod]
    public void UpdateQuantity_FailDueToUnknownProduce()
    {
        //ARRANGE
        LoadTestData();
        Mock<IHttpContextAccessor> mockContextAccessor = new Mock<IHttpContextAccessor>();
        DefaultHttpContext context = new DefaultHttpContext();
        mockContextAccessor.Setup(m => m.HttpContext)
            .Returns(context);
        mockContextAccessor.Setup(m => m.HttpContext!.Request)
            .Returns(context.Request);
        mockContextAccessor.Setup(m => m.HttpContext!.Response)
            .Returns(context.Response);
        mockContextAccessor.Setup(m => m.HttpContext!.Request.Cookies)
            .Returns(context.Request.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.Response.Cookies)
            .Returns(context.Response.Cookies);
        mockContextAccessor.Setup(m => m.HttpContext!.User.Identity!.IsAuthenticated)
            .Returns(true);

        Mock<ISession> mockSession = new Mock<ISession>();

        //I was having trouble figuring out how to handle testing around ISession
        //The Murach textbook talks about this time, but in a limited capacity
        //I asked Claude how people can handle mimicing Get/Set with byte data, and it showed me code simialar to this
        //I am implementing it here, it should make the mock store/ retrieve like a dictionary
        Dictionary<string, byte[]> sessionStorage = new Dictionary<string, byte[]>();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
        .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

        mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                var found = sessionStorage.TryGetValue(key, out value!);
                return found;
            });
        mockContextAccessor.Setup(m => m.HttpContext!.Session)
                .Returns(mockSession.Object);

        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();

        ICartService cartService = new CartService(mockContextAccessor.Object, mockProduceRepo.Object, mockDiscountRepo.Object);
        //Add single item to cart
        cartService.AddItem(testCartItems[0]);

        //ACT
        cartService.UpdateQuantity(153, 100); //Invalid produce

        //ASSERT
        List<CartItem> cart = cartService.GetCart();
        //Should be same as before test
        Assert.IsTrue(cart.Count == 1);
        Assert.IsFalse(cart.Any(ci => ci.ProduceId == 153));
    }
}
