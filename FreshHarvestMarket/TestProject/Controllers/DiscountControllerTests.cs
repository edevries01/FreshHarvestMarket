using FreshHarvestMarket.Controllers;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net.Sockets;

namespace TestProject;

[TestClass]
public class DiscountControllerTests
{
    //Copied the seed data from FreshHarvestContext
    IQueryable<Produce> testProduce = new List<Produce> {new Produce()
                {
                    ProduceId = 1,
                    ProduceName = "Zucchini",
                    ProduceDescription = "Fresh green zucchini from Williamsburg, Iowa",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 20,
                    ImageUrl = "zucchini.jpg"
                },
                new Produce()
                {
                    ProduceId = 2,
                    ProduceName = "Tomatoes",
                    ProduceDescription = "Ripe red tomatoes from Victor, Iowa",
                    UnitPrice = 2.00m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25,
                    ImageUrl = "tomatoes.jpg"
                },
                new Produce()
                {
                    ProduceId = 3,
                    ProduceName = "Honey",
                    ProduceDescription = "Local raw honey from Victor, Iowa",
                    UnitPrice = 6.00m,
                    ProduceCategory = "Other",
                    InventoryTotal = 15,
                    ImageUrl = "honey.jpg"
                },
                new Produce()
                {
                    ProduceId = 4,
                    ProduceName = "Plums",
                    ProduceDescription = "Juicy plums from Amana, Iowa",
                    UnitPrice = 2.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 18,
                    ImageUrl = "plums.jpg"
                },
                new Produce()
                {
                    ProduceId = 5,
                    ProduceName = "Potatoes",
                    ProduceDescription = "Freshly harvested potatoes from Ladora, Iowa",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 30,
                    ImageUrl = "potatoes.jpg"
                },
                new Produce()
                {
                    ProduceId = 6,
                    ProduceName = "Blueberries",
                    ProduceDescription = "Sweet blueberries from Tiffin, Iowa",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12,
                    ImageUrl = "blueberries.jpg"
                },
                new Produce()
                {
                    ProduceId = 7,
                    ProduceName = "Sweet Corn",
                    ProduceDescription = "Fresh sweet corn from Marengo, Iowa",
                    UnitPrice = 1.75m,
                    InventoryTotal = 20,
                    ImageUrl = "sweetcorn.jpg"
                },
                new Produce()
                {
                    ProduceId = 8,
                    ProduceName = "Broccoli",
                    ProduceDescription = "Green broccoli florets from Millersburg, Iowa",
                    UnitPrice = 2.25m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 15,
                    ImageUrl = "broccoli.jpg"
                },
                new Produce()
                {
                    ProduceId = 9,
                    ProduceName = "Garlic",
                    ProduceDescription = "Fresh garlic bulbs from Montezuma, Iowa",
                    UnitPrice = 0.80m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 40,
                    ImageUrl = "garlic.jpg"
                },
                new Produce()
                {
                    ProduceId = 10,
                    ProduceName = "Cherries",
                    ProduceDescription = "Sweet red cherries from Solon, Iowa",
                    UnitPrice = 3.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 10,
                    ImageUrl = "cherries.jpg"
                },
                new Produce()
                {
                    ProduceId = 11,
                    ProduceName = "Carrots",
                    ProduceDescription = "Organic carrots from Kalona, Iowa",
                    UnitPrice = 1.50m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25,
                    ImageUrl = "carrots.jpg"
                },
                new Produce()
                {
                    ProduceId = 12,
                    ProduceName = "Raspberries",
                    ProduceDescription = "Fresh raspberries from Swisher, Iowa",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12,
                    ImageUrl = "raspberries.jpg"
                } }.AsQueryable();

    IQueryable<Discount> testDiscounts = new List<Discount>() {new Discount
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

    //Had to just assign everything manually since .Load() leads to nullreference exception on IQueryable
    private void LoadProperties()
    {
        foreach (Discount discount in testDiscounts) 
        {
            discount.Produce = testProduce.FirstOrDefault(p => p.ProduceId == discount.ProduceId);
        }

    }

    [TestMethod]
    public void Index_Get_ReturnViewSuccessfully()
    {
        LoadProperties();

        //Arrange
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();
        mockDiscountRepo.Setup(r => r.GetAll()).Returns(testDiscounts);
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(r => r.GetAll()).Returns(testProduce);

        DiscountController discountController = new DiscountController(mockDiscountRepo.Object, mockProduceRepo.Object);

        //Act
        var testResult = discountController.Index();

        //Assert
        //Should return a ViewResult
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        //Should have List<Discount> as the model
        ViewResult testResultView = (ViewResult)testResult;
        Assert.IsNotNull(testResultView.Model);
        Assert.IsInstanceOfType(testResultView.Model, typeof(List<Discount>));

        mockDiscountRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [TestMethod]
    public void Add_Get_ReturnAddEditViewSuccessfully()
    {
        LoadProperties();

        //ARRANGE
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();
        mockDiscountRepo.Setup(r => r.GetAll()).Returns(testDiscounts);
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(r => r.GetAll()).Returns(testProduce);

        DiscountController discountController = new DiscountController(mockDiscountRepo.Object, mockProduceRepo.Object);

        //ACT
        var testResult = discountController.Add();

        //ASSERT
        //Should return a ViewResult
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        //Should have Discount as the model
        ViewResult testResultView = (ViewResult)testResult;
        Assert.IsNotNull(testResultView.Model);
        Assert.IsInstanceOfType(testResultView.Model, typeof(Discount));

        //We don't put any produce items that already have a discount in the options
        Assert.IsNotNull(testResultView.ViewData["Produce"]);
        Assert.IsInstanceOfType(testResultView.ViewData["Produce"], typeof(List<Produce>));
        List<Produce> viewProduce = (List<Produce>)testResultView.ViewData["Produce"]!;
        //We have a produce with an ID of 1 already, so it should not have been added to the viwe
        Assert.IsFalse(viewProduce.Any(p => p.ProduceId == 1));
        //This one should be in there
        Assert.IsTrue(viewProduce.Any(p => p.ProduceId == 2));

        //Verify calls
        mockProduceRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [TestMethod]
    public void Edit_Get_ReturnAddEditViewSuccessfully() 
    {
        LoadProperties();

        //ARRANGE
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();
        mockDiscountRepo.Setup(r => r.GetAll()).Returns(testDiscounts);
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(r => r.GetAll()).Returns(testProduce);

        DiscountController discountController = new DiscountController(mockDiscountRepo.Object, mockProduceRepo.Object);

        //ACT
        var testResult = discountController.Edit(1);

        //ASSERT
        //Should return a ViewResult
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        //Should have Discount as the model
        ViewResult testResultView = (ViewResult)testResult;
        Assert.IsNotNull(testResultView.Model);
        Assert.IsInstanceOfType(testResultView.Model, typeof(Discount));

        //Verify the right item is on the model
        Discount testResultModel = (Discount)testResultView.Model;
        Assert.IsTrue(testResultModel.DiscountId == 1);
        Assert.IsTrue(testResultModel.DiscountAmount == 50);

        //Verify calls
        mockProduceRepo.Verify(r => r.GetAll(), Times.Once);
        mockDiscountRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [TestMethod]
    public void Edit_Post_UpdateDiscountSuccessfully()
    {
        LoadProperties();

        //ARRANGE
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();
        mockDiscountRepo.Setup(r => r.GetAll()).Returns(testDiscounts);
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(r => r.GetAll()).Returns(testProduce);

        DiscountController discountController = new DiscountController(mockDiscountRepo.Object, mockProduceRepo.Object);

        Discount discountToEdit = new Discount() {
            DiscountId = 1,
            DiscountAmount = 50,
            ProduceId = 1
        };
        discountToEdit.DiscountAmount = 32;


        //ACT
        var testResult = discountController.Edit(discountToEdit);

        //ASSERT
        //Should return a ViewResult (rerouted to index)
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(RedirectToActionResult));


        //Verify calls
        mockDiscountRepo.Verify(r => r.Update(discountToEdit), Times.Once);
        mockDiscountRepo.Verify(r => r.Save(), Times.Once);
    }

    [TestMethod]
    public void Edit_Post_FailModelValidation()
    {
        LoadProperties();

        //ARRANGE
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();
        mockDiscountRepo.Setup(r => r.GetAll()).Returns(testDiscounts);
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(r => r.GetAll()).Returns(testProduce);

        DiscountController discountController = new DiscountController(mockDiscountRepo.Object, mockProduceRepo.Object);

        Discount discountToEdit = new Discount()
        {
            DiscountId = 1,
            DiscountAmount = 50,
            ProduceId = 1
        };
        discountToEdit.DiscountAmount = 32;
        discountController.ModelState.AddModelError("ModelValidationError", "I am an error, and nothing less!");

        //ACT
        var testResult = discountController.Edit(discountToEdit);

        //ASSERT
        //Should return a ViewResult
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(ViewResult));

        ViewResult testResultView = (ViewResult)testResult;
        Assert.IsNotNull(testResultView.Model);
        Assert.IsInstanceOfType(testResultView.Model, typeof(Discount));

        //Verify the right item is on the model
        Discount testResultModel = (Discount)testResultView.Model;
        Assert.IsTrue(testResultModel.DiscountId == 1);
        Assert.IsTrue(testResultModel.DiscountAmount == 32);
    }

    /// <summary>
    /// When a POST is sent to the Discount Controller, should successfully delete the discount
    /// </summary>
    [TestMethod]
    public void Delete_Post_DeleteDiscountSuccessfully()
    {
        LoadProperties();

        //ARRANGE
        Mock<IRepository<Discount>> mockDiscountRepo = new Mock<IRepository<Discount>>();
        mockDiscountRepo.Setup(r => r.GetAll()).Returns(testDiscounts);
        Mock<IRepository<Produce>> mockProduceRepo = new Mock<IRepository<Produce>>();
        mockProduceRepo.Setup(r => r.GetAll()).Returns(testProduce);

        DiscountController discountController = new DiscountController(mockDiscountRepo.Object, mockProduceRepo.Object);

        Discount discountToDelete = testDiscounts.FirstOrDefault(d => d.DiscountId == 1)!;

        //Act
        var testResult = discountController.Delete(1);

        //Assert
        //Should return a ViewResult
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType(testResult, typeof(RedirectToActionResult));

        //Verify calls
        mockDiscountRepo.Verify(r => r.Delete(discountToDelete), Times.Once);
        mockDiscountRepo.Verify(r => r.Save(), Times.Once);
    }
}
