/// <summary>
/// Unit tests for the OrderItem model.
/// 
/// Verifies calculations related to individual order items,
/// such as computing the total price based on produce price
/// & quantity.
/// </summary>

using FreshHarvestMarket.Models;

namespace TestProject;

[TestClass]
public class OrderItemTests
{
    [TestMethod]
    public void OrderItem_TotalPrice()
    {
        //ARRANGE
        OrderItem orderItem = new OrderItem
        {
            OrderItemId = 1,
            OrderId = 1,
            Order = new Order()
            {
                OrderId = 1,
                OrderTotal = 12.50m,
                OrderDate = new DateTime(2026, 3, 1),
                PickupDate = new DateTime(2026, 3, 2),
                IsPickedUp = false,
                Rejected = false
            },
            ProduceId = 1,
            Produce = new Produce()
            {
                ProduceId = 1,
                ProduceName = "Zucchini",
                ProduceDescription = "Fresh green zucchinis",
                UnitPrice = 1.20m,
                ProduceCategory = "Vegetable",
                InventoryTotal = 20
            },
            Quantity = 3
        };

        //ACT
        decimal totalPrice = orderItem.TotalPrice;

        //ASSERT
        Assert.AreEqual(3.60m, totalPrice);
    }
}
