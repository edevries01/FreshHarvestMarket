using FreshHarvestMarket.Models;

namespace TestProject;

[TestClass]
public class CartItemTests
{
    [TestMethod]
    [DataRow(10, 1.20, 50, 00.60)]
    [DataRow(10, 1.20, 0, 1.20)]
    public void DiscountedPrice_Successful(int quantity, double price, int discountAmount, double expectedTestAmount)
    {
        //ARRANGE
        decimal priceDecimal = (decimal)price;
        decimal expectedTestAmountDecimal = (decimal)expectedTestAmount;

        CartItem cartItem = new CartItem()
        {
            ProduceId = 1,
            Quantity = quantity,
            ProduceName = "Zuccihini",
            Price = priceDecimal,
            DiscountAmount = discountAmount
        };

        //ACT
        decimal discountedPrice = cartItem.DiscountedPrice;

        //ASSERT
        Assert.AreEqual(expectedTestAmountDecimal, discountedPrice);
    }
}
