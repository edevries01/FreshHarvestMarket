/// <summary>
/// Contains unit tests for the HomeController class.
/// Verifies that each action returns the expected ViewResult for basic navigation pages
/// such as Index, Login, About, & Privacy.
/// </summary>

using FreshHarvestMarket.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject;

[TestClass]
public class HomeControllerTests
{
    /// <summary>
    /// Scenarios that expect success
    /// when calling a 'GET' request on the 'Home' controller with the 'Index' action
    /// </summary>
    [TestMethod]
    public void HomeController_Index()
    {
        //Arrange
        ILogger<HomeController> nullLogger = new NullLogger<HomeController>(); //Not verifying logging... change if desired
        HomeController testController = new HomeController(nullLogger);

        //Act
        IActionResult testResult = testController.Index();

        //Assert
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        //If we make it this far, should be a ViewResult
        ViewResult testResultViewResult = (ViewResult)testResult;

        ViewResult testViewResult = (ViewResult)testResultViewResult;
        Assert.AreEqual(null, testViewResult.ViewName);
    }

    /// <summary>
    /// Scenarios that expect success
    /// when calling a 'GET' request on the 'Home' controller with the 'Login' action
    /// </summary>
    [TestMethod]
    public void HomeController_GetLogin()
    {
        //Arrange
        ILogger<HomeController> nullLogger = new NullLogger<HomeController>(); //Not verifying logging... change if desired
        HomeController testController = new HomeController(nullLogger);

        //Act
        IActionResult testResult = testController.Login();

        //Assert
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        //If we make it this far, should be a ViewResult
        ViewResult testResultViewResult = (ViewResult)testResult;

        ViewResult testViewResult = (ViewResult)testResultViewResult;
        Assert.AreEqual(null, testViewResult.ViewName);
    }

    /// <summary>
    /// Scenarios that expect success
    /// when calling a 'GET' request on the 'Home' controller with the 'About' action
    /// </summary>
    [TestMethod]
    public void HomeController_GetAbout()
    {
        //Arrange
        ILogger<HomeController> nullLogger = new NullLogger<HomeController>(); //Not verifying logging... change if desired
        HomeController testController = new HomeController(nullLogger);

        //Act
        IActionResult testResult = testController.Login();

        //Assert
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        //If we make it this far, should be a ViewResult
        ViewResult testResultViewResult = (ViewResult)testResult;

        ViewResult testViewResult = (ViewResult)testResultViewResult;
        Assert.AreEqual(null, testViewResult.ViewName);
    }

    /// <summary>
    /// Scenarios that expect success
    /// when calling a 'GET' request on the 'Home' controller with the 'About' action
    /// </summary>
    [TestMethod]
    public void HomeController_Privacy()
    {
        //Arrange
        ILogger<HomeController> nullLogger = new NullLogger<HomeController>(); //Not verifying logging... change if desired
        HomeController testController = new HomeController(nullLogger);

        //Act
        IActionResult testResult = testController.Login();

        //Assert
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        //If we make it this far, should be a ViewResult
        ViewResult testResultViewResult = (ViewResult)testResult;

        ViewResult testViewResult = (ViewResult)testResultViewResult;
        Assert.AreEqual(null, testViewResult.ViewName);
    }
}
