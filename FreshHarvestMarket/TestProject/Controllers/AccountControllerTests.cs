using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace TestProject.Controllers;

[TestClass]
public class AccountControllerTests
{
    //Sign-in manager is an even bigger pain-in-the-butt to mock that UserManager
    //Useful article if you need other examples of people mocking this thing
    //https:///stackoverflow.com/questions/48189741/mocking-a-signinmanager*/

    /// <summary>
    /// GET call on Register we expect to be successful
    /// </summary>
    [TestMethod]
    public void Register_Successful()
    {
        //ARRANGE
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);


        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        //ACT
        var result = accountController.Register();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public void Post_Register_Successful()
    {
        //ARRANGE
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);



        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        RegisterViewModel testViewModel = new RegisterViewModel()
        {
            Username = "BobShopper",
            Firstname = "Bob",
            Lastname = "Shopper",
            Email = "BobShopper@mail.com",
            Password = "Capybara",
            ConfirmPassword = "Capybara"
        };


        //ACT
        var result = accountController.Register(testViewModel).GetAwaiter().GetResult();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
        Assert.IsTrue(redirectToActionResult.ControllerName == "Account");
        Assert.IsTrue(redirectToActionResult.ActionName == "Index");


        //AssertCalls
        mockUserManager.Verify(m => m.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    public void Post_Register_UnsuccessfulModelStateInvalid()
    {
        //ARRANGE
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);



        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);
        accountController.ModelState.AddModelError("fake key", "fake error message");

        RegisterViewModel testViewModel = new RegisterViewModel()
        {
            Username = "BobShopper",
            Firstname = "Bob",
            Lastname = "Shopper",
            Email = "BobShopper@mail.com",
            Password = "Capybara",
            ConfirmPassword = "Capybara"
        };


        //ACT
        var result = accountController.Register(testViewModel).GetAwaiter().GetResult();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));


        //AssertCalls
        mockUserManager.Verify(m => m.CreateAsync(It.IsAny<User>()), Times.Never());
    }

    [TestMethod]
    public void LogOut_Successful()
    {
        //ARRANGE
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);

        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        //ACT
        var result = accountController.LogOut().GetAwaiter().GetResult();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        mockSignInManager.Verify(m => m.SignOutAsync(), Times.Once());
    }

    [TestMethod]
    public void Login_Successful()
    {
        //ARRANGE
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);

        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        //ACT
        var result = accountController.Login();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }

    /*[TestMethod]
    public void Post_Login_Successful()
    {
        //ARRANGE
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);

        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        //ACT
        var result = accountController.Login();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }*/
}
