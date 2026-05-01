/*
 * ConfirmationViewModel.cs
 * FreshHarvestMarket
 *
 * This ViewModel is used to display order confirmation details
 * after a customer successfully places an order.
 *
 * It contains customer information, order metadata, & the list
 * of items included in the order.
 *
 * It also calculates the total cost of the order, applying discounts
 * when applicable to each item.
 */

using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.ViewModels
{
    public class ConfirmationViewModel
    {
        public int OrderId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public DateTime PickupDate { get; set; }

        public List<CartItem> Items { get; set; } = new();

        public decimal Total => Items.Sum(x =>
            (x.DiscountAmount.HasValue && x.DiscountAmount > 0)
                ? x.DiscountedPrice * x.Quantity
                : x.Price * x.Quantity
        );
    }
}