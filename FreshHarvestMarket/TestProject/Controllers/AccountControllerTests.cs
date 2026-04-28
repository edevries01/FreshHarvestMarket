using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Services;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
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

    /// <summary>
    /// Test for successful login with a redirect url
    /// </summary>
    [TestMethod]
    public void Post_Login_SuccessfulAndRedirect()
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
        mockSignInManager
            .Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .   ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        //We have to set this attribute or Url.IsLocalUrl() will fail
        accountController.Url = new UrlHelper(new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor()));

        LoginViewModel loginViewModel = new LoginViewModel()
        {
            Username = "BobShopper",
            Password = "Capybara",
            ReturnUrl = @"/home/index",
            RememberMe = true
        };

        //ACT
        var result = accountController.Login(loginViewModel).GetAwaiter().GetResult();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectResult));
        RedirectResult redirectResult = (RedirectResult)result;
        Assert.IsTrue(redirectResult.Url == "/home/index");

        //Assert calls
        mockSignInManager.Verify(m => m.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()), Times.Once);
    }

    /// <summary>
    /// Test with successful login where we don't have a redirect url
    /// </summary>
    [TestMethod]
    public void Post_Login_SuccessfulAndDefaultRedirect()
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
        mockSignInManager
            .Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        //We have to set this attribute or Url.IsLocalUrl() will fail
        accountController.Url = new UrlHelper(new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor()));

        LoginViewModel loginViewModel = new LoginViewModel()
        {
            Username = "BobShopper",
            Password = "Capybara",
            ReturnUrl = null,
            RememberMe = true
        };

        //ACT
        var result = accountController.Login(loginViewModel).GetAwaiter().GetResult();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        RedirectToActionResult redirectResult = (RedirectToActionResult)result;
        Assert.IsTrue(redirectResult.ControllerName == "Home");
        Assert.IsTrue(redirectResult.ActionName == "Index");

        //Assert calls
        mockSignInManager.Verify(m => m.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()), Times.Once);
    }

    /// <summary>
    /// Test with unsuccessful login
    /// </summary>
    [TestMethod]
    public void Post_Login_Unsuccessful()
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
        mockSignInManager
            .Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object);

        //We have to set this attribute or Url.IsLocalUrl() will fail
        accountController.Url = new UrlHelper(new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor()));

        LoginViewModel loginViewModel = new LoginViewModel()
        {
            Username = "BobShopper",
            Password = "Capybara",
            ReturnUrl = null,
            RememberMe = true
        };

        //ACT
        var result = accountController.Login(loginViewModel).GetAwaiter().GetResult();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        ViewResult viewResult = (ViewResult)result;
        Assert.IsNotNull(viewResult.Model);
        Assert.IsInstanceOfType(viewResult.Model, typeof(LoginViewModel));
        

        //Assert calls
        mockSignInManager.Verify(m => m.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()), Times.Once);
    }

    /// <summary>
    /// Expects to return the access denied view successfully
    /// </summary>
    [TestMethod]
    public void AccessDenied_Successful()
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
        var result = accountController.AccessDenied();

        //ASSERT
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }
}
