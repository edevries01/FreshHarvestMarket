using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.OtherServices;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject;

[TestClass]
public class OrderFiltersControllerTests
{
    /// <summary>
    /// Call the index method successfully
    /// </summary>
    [TestMethod]
    public void Index_Successful()
    {
        //ARRANGE
        Mock<IOrderFiltersSession> mockOrderFiltersSession = new Mock<IOrderFiltersSession>();
        OrderFiltersController testController = new OrderFiltersController(mockOrderFiltersSession.Object);

        BrowseOrdersViewModel testViewModel = new BrowseOrdersViewModel();

        //ACT
        var result = testController.Index(testViewModel);

        //ASSERT
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Assert redirect is as expected
        RedirectToActionResult redirectResult = (RedirectToActionResult)result;
        Assert.AreEqual("Index", redirectResult.ActionName);
        Assert.AreEqual("Order", redirectResult.ControllerName);

        //Assert service calls
        mockOrderFiltersSession.Verify(s => s.SetFilters(testViewModel), Times.Once);
    }
}
