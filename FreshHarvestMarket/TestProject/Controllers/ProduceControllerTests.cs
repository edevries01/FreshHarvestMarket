using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject;

[TestClass]
public class ProduceControllerTests
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
    private IQueryable<Favorite> testFavorites = new List<Favorite>() 
    {
        new Favorite
        {
            FavoriteId =  1,
            ProduceId = 1
        }
    }.AsQueryable();

    private void LoadMockData()
    {
        foreach (Favorite favorite in testFavorites) 
        {
            favorite.Produce = testProduce.FirstOrDefault(p => p.ProduceId == favorite.ProduceId)!;
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

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();
        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object);

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

        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();
        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);

        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();

        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object);

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
        Mock<IRepository<Produce>> _produceRepoMock = new Mock<IRepository<Produce>>();
        _produceRepoMock.Setup(m => m.GetAll()).Returns(testProduce);
        Mock<IRepository<Favorite>> _favoriteRepoMock = new Mock<IRepository<Favorite>>();
        ProduceController testController = new ProduceController(_produceRepoMock.Object, _favoriteRepoMock.Object);

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

        //ACT

        //ASSERT
    }
}
