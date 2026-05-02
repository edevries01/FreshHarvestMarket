/// <summary>
/// Unit tests for the ProduceController.
/// 
/// Verifies behavior for retrieving, filtering, creating, editing,
/// & deleting produce items, as well as managing favorites.
/// Uses mocked repositories to isolate controller logic & ensure
/// correct interactions with the data layer.
/// </summary>

using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TestProject;

[TestClass]
public class ProduceControllerTests
{
    private Mock<UserManager<User>> GetMockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    private void SetupUser(Controller controller)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.NameIdentifier, "test-user")
            }, "mock"));

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

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

    private IQueryable<Favorite> GetTestFavorites()
    {
        return new List<Favorite>
    {
        new Favorite
        {
            FavoriteId = 1,
            ProduceId = 1,
            UserId = "test-user"
        }
    }.AsQueryable();
    }

private void LoadMockData()
{
    var favorites = GetTestFavorites();

    foreach (Favorite favorite in favorites)
    {
        favorite.Produce = testProduce
            .FirstOrDefault(p => p.ProduceId == favorite.ProduceId)!;
    }
}

    /// <summary>
    /// Simulates a GET request hitting the index action-method successfully
    /// </summary>
    [TestMethod]
    public void Get_Index_ReturnViewSuccessfully()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();
        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Index();

        //ASSERT
        //Verify its not null
        Assert.IsNotNull(result);

        //Verify of type ViewResult
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        ViewResult viewResult = (ViewResult)result;

        //Verify stuff with model
        Assert.IsNotNull(viewResult.Model);
        Assert.IsInstanceOfType(viewResult.Model, typeof(BrowseProduceViewModel));
        BrowseProduceViewModel viewViewModel = (BrowseProduceViewModel)viewResult.Model;

        //Verify nothing was filtered out since default filter should be ALL
        Assert.IsFalse(viewViewModel.ProduceItems.Any(p => !testProduce.Contains(p)));
    }

    /// <summary>
    /// Sends a GET request to the Index action-method to test filtering on "vegetable"
    /// </summary>
    [TestMethod]
    public void Get_Index_FilterCorrectly()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();
        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Index("Vegetable");

        //ASSERT
        //Verify its not null
        Assert.IsNotNull(result);

        //Verify of type ViewResult
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        ViewResult viewResult = (ViewResult)result;

        //Verify stuff with model
        Assert.IsNotNull(viewResult.Model);
        Assert.IsInstanceOfType(viewResult.Model, typeof(BrowseProduceViewModel));
        BrowseProduceViewModel viewViewModel = (BrowseProduceViewModel)viewResult.Model;

        //Verify filter worked correctly
        Assert.IsTrue(viewViewModel.ProduceItems.All(p => p.ProduceCategory == "Vegetable"));
    }

    [TestMethod]
    public void Get_Details_Successfully()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();
        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();
        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Details(1);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type ViewResult
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        ViewResult viewResult = (ViewResult)result;

        //Verify stuff with model
        Assert.IsNotNull(viewResult.Model);
        Assert.IsInstanceOfType(viewResult.Model, typeof(Produce));
        Produce viewViewModel = (Produce)viewResult.Model;

        Assert.IsTrue(viewViewModel.ProduceId == 1);
    }

    [TestMethod]
    public void Get_Details_Unsuccessfully()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();
        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();
        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Details(10000);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type NotFound
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    /// <summary>
    /// Test where we successfully add a favorite
    /// </summary>
    [TestMethod]
    public void Post_AddFavorite_Successfully()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                   .Returns("test-user");

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();
        _favoriteRepoMock.Setup(m => m.GetAll()).Returns(GetTestFavorites());

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        SetupUser(testController);

        //ACT
        var result = testController.AddFavorite(2);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type Redirect
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        RedirectToActionResult viewResult = (RedirectToActionResult)result;

        //Verify Calls
        _favoriteRepoMock.Verify(m =>
            m.Insert(It.IsAny<Favorite>()),
            Times.Once);

        _favoriteRepoMock.Verify(m => m.Save(), Times.Once);
    }

    /// <summary>
    /// Test where we don't add a favorite cause it already exists
    /// </summary>
    [TestMethod]
    public void Post_AddFavorite_Unsuccessfully()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                   .Returns("test-user");

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();
        _favoriteRepoMock.Setup(m => m.GetAll()).Returns(GetTestFavorites());

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        SetupUser(testController);

        //ACT
        var result = testController.AddFavorite(1);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type Redirect
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        RedirectToActionResult viewResult = (RedirectToActionResult)result;

        //Verify Calls
        _favoriteRepoMock.Verify(m => m.Insert(It.IsAny<Favorite>()), Times.Never());
        _favoriteRepoMock.Verify(m => m.Save(), Times.Never());
    }

    /// <summary>
    /// We test if a post request was sent to delete that is expected to be successful
    /// </summary>
    [TestMethod]
    public void Post_DeleteFavorite_Successfully()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                   .Returns("test-user");

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        _favoriteRepoMock.Setup(m => m.GetAll()).Returns(GetTestFavorites());

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        SetupUser(testController);

        //ACT
        var result = testController.DeleteFavorite(1);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type Redirect
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        RedirectToActionResult viewResult = (RedirectToActionResult)result;

        //Verify Calls
        _favoriteRepoMock.Verify(m => m.Delete(It.IsAny<Favorite>()), Times.Once);
        _favoriteRepoMock.Verify(m => m.Save(), Times.Once);
    }
    
    //We test where a post method is sent in to delete a favorite for a produce that is not favorited
    [TestMethod]
    public void Post_DeleteFavorite_Unsuccessfully()
    {
        //ARRANGE
        LoadMockData();

        var userManagerMock = GetMockUserManager();

        userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                   .Returns("test-user");

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        _favoriteRepoMock.Setup(m => m.GetAll()).Returns(GetTestFavorites());

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        SetupUser(testController);

        //ACT
        var result = testController.DeleteFavorite(2);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type Redirect
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        RedirectToActionResult viewResult = (RedirectToActionResult)result;

        //Verify Calls
        _favoriteRepoMock.Verify(m => m.Delete(It.IsAny<Favorite>()), Times.Never());
        _favoriteRepoMock.Verify(m => m.Save(), Times.Never());
    }

    /// <summary>
    /// Successfully return create page
    /// </summary>
    [TestMethod]
    public void Get_Create_Successfully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Create();

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type Redirect
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }

    /// <summary>
    /// Test where we expect to successfully create produce
    /// </summary>
    [TestMethod]
    public void Post_Create_CreateProduceSuccessfully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        Produce testProduceVar = testProduce.FirstOrDefault(p => p.ProduceId == 1)!;

        //ACT
        var result = testController.Create(testProduceVar);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type Redirect
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Verify calls on services
        _produceRepoMock.Verify(m => m.Insert(testProduceVar), Times.Once);
        _produceRepoMock.Verify(m => m.Save(), Times.Once);
    }

    /// <summary>
    /// Test where we expect produce creation to fail
    /// </summary>
    [TestMethod]
    public void Post_Create_CreateProduceUnsuccessfully() 
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        Produce testProduceVar = testProduce.FirstOrDefault(p => p.ProduceId == 1)!;

        testController.ModelState.AddModelError("fakeerror", "fake error message");

        //ACT
        var result = testController.Create(testProduceVar);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type ViewResult
        Assert.IsInstanceOfType(result, typeof(ViewResult));

        //Verify calls on services
        _produceRepoMock.Verify(m => m.Insert(testProduceVar), Times.Never());
        _produceRepoMock.Verify(m => m.Save(), Times.Never());
    }

    /// <summary>
    /// Expects to get the edit page for a specific produce successfully
    /// </summary>
    [TestMethod]
    public void Get_Edit_GetEditPageSuccessully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Edit(1);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type ViewResult
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        ViewResult viewResult = (ViewResult)result;

        //Verify correct item in model
        Assert.IsNotNull(viewResult.Model);
        var viewModel = viewResult.Model;
        Assert.IsInstanceOfType(viewModel, typeof(Produce));
        Produce produceModel = (Produce)viewResult.Model;
        Assert.IsTrue(produceModel.ProduceId == 1);

        //Verify calls on services
        _produceRepoMock.Verify(m => m.GetAll(), Times.Once);
    }

    /// <summary>
    /// Expects to get the edit page for a specific produce unsuccessfully
    /// </summary>
    [TestMethod]
    public void Get_Edit_GetEditPageUnsuccessully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Edit(5);

        //ASSERT
        Assert.IsNotNull(result);

        //Verify of type ViewResult
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    /// <summary>
    /// Expects to edit a specific item successfully
    /// </summary>
    [TestMethod]
    public void Post_Edit_EditItemSuccessfully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        Produce testProduceVar = testProduce.FirstOrDefault(p => p.ProduceId == 1)!;

        //ACT
        var result = testController.Edit(testProduceVar);

        //Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Assert calls
        _produceRepoMock.Verify(m => m.Update(testProduceVar), Times.Once);
        _produceRepoMock.Verify(m => m.Save(), Times.Once);

    }

    /// <summary>
    /// Expects to edit a specific item unsuccessfully
    /// </summary>
    [TestMethod]
    public void Post_Edit_EditItemUnsuccessfully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        Produce testProduceVar = testProduce.FirstOrDefault(p => p.ProduceId == 1)!;
        testController.ModelState.AddModelError("fakeerror", "fake error message");

        //ACT
        var result = testController.Edit(testProduceVar);

        //ASSERT
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(ViewResult));

        //Verify calls
        _produceRepoMock.Verify(m => m.Update(testProduceVar), Times.Never());
        _produceRepoMock.Verify(m => m.Save(), Times.Never());
    }

    /// <summary>
    /// Scenario where we expect to delete successfully
    /// </summary>
    [TestMethod]
    public void Post_Delete_DeleteItemSuccessfully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager();

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Delete(1);

        //Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

        //Assert calls
        _produceRepoMock.Verify(m => m.Delete(It.Is<Produce>(p => p.ProduceId == 1)), Times.Once());
        _produceRepoMock.Verify(m => m.Save(), Times.Once);
    }

    [TestMethod]
    public void Post_Delete_DeleteItemUnsuccessfully()
    {
        //ARRANGE
        var userManagerMock = GetMockUserManager(); 

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();

        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object,
        userManagerMock.Object);

        //ACT
        var result = testController.Delete(5);

        //Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = (RedirectToActionResult)result;
        Assert.AreEqual("ManageProduce", redirect.ActionName);

        //Assert calls
        _produceRepoMock.Verify(m => m.Delete(It.IsAny<Produce>()), Times.Never());
        _produceRepoMock.Verify(m => m.Save(), Times.Never());
    }
}
