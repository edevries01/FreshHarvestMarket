/// <summary>
/// Tests for the CartController
/// 
/// Includes scenarios for adding, removing, & updating cart items,
/// as well as viewing the cart & checkout page.
/// </summary>

using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject.Controllers;

[TestClass]
public class CartControllerTests
{
    /// <summary>
    /// GET call on Index we expect to be successful
    /// </summary>
    [TestMethod]
    public void Index_Successful()
    {
        //ARRANGE
        Mock<ICartService> mockCartService = new Mock<ICartService>();
        mockCartService.Setup(m => m.GetCart()).Returns(new List<CartItem>());
        mockCartService.Setup(m => m.GetTotal()).Returns(0);

        CartController cartController = new CartController(mockCartService.Object);

        //ACT
        var result = cartController.Index();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));

        ViewResult viewResult = (ViewResult)result;

        //Assert calls
        mockCartService.Verify(s => s.GetCart(), Times.Once());
        mockCartService.Verify(s => s.GetTotal(), Times.Once());    
    }

    [TestMethod]
    public void Add_Successfully()
    {
        //ARRANGE
        Mock<ICartService> mockCartService = new Mock<ICartService>();

        CartController cartController = new CartController(mockCartService.Object);

        //ACT
        var result = cartController.Add(1, "Carrot", "image.jpg", 1.00m, 1);

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Assert calls
        mockCartService.Verify(s => s.AddItem(It.IsAny<CartItem>()), Times.Once());
    }

    /// <summary>
    /// Verifies we call remove successfully
    /// </summary>
    [TestMethod]
    public void Remove_Successfully()
    {
        //ARRANGE
        Mock<ICartService> mockCartService = new Mock<ICartService>();

        CartController cartController = new CartController(mockCartService.Object);

        //ACT
        var result = cartController.Remove(1);

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        mockCartService.Verify(m => m.RemoveItem(1), Times.Once);
    }

    /// <summary>
    /// Verify we call the GET path on Checkout() correctly
    /// </summary>
    [TestMethod]
    public void Checkout_Successful()
    {
        //ARRANGE
        Mock<ICartService> mockCartService = new Mock<ICartService>();

        CartController cartController = new CartController(mockCartService.Object);

        //ACT
        var result = cartController.Checkout();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));

        ViewResult viewResult = (ViewResult)result;
    }

    [TestMethod]
    public void Update_Successfully()
    {
        //ARRANGE
        Mock<ICartService> mockCartService = new Mock<ICartService>();
        mockCartService.Setup(m => m.GetCart()).Returns(new List<CartItem>() { new CartItem() { ProduceId = 1, Quantity = 10, MaxQuantity = 20} });
        mockCartService.Setup(m => m.GetTotal()).Returns(0);

        CartController cartController = new CartController(mockCartService.Object);

        //ACT
        var result = cartController.Update(1, 12);

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Assert calls
        mockCartService.Verify(m => m.GetCart(), Times.Once());
        mockCartService.Verify(m => m.UpdateQuantity(1, 12), Times.Once());
    }

    //Should fail to update the item since it is not in the cart, but otherwise be the same as the successful one
    [TestMethod]
    public void Update_UnsuccessfulBecauseItemNotFound()
    {
        //ARRANGE
        Mock<ICartService> mockCartService = new Mock<ICartService>();
        mockCartService.Setup(m => m.GetCart()).Returns(new List<CartItem>() { new CartItem() { ProduceId = 1, Quantity = 10, MaxQuantity = 20 } });
        mockCartService.Setup(m => m.GetTotal()).Returns(0);

        CartController cartController = new CartController(mockCartService.Object);

        //ACT
        var result = cartController.Update(125, 1);

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Assert calls
        mockCartService.Verify(m => m.GetCart(), Times.Once());
        mockCartService.Verify(m => m.UpdateQuantity(125, 1), Times.Never());
    }
}
