using FreshHarvestMarket.Models;

namespace FreshHarvestMarket.ViewModels
{
    public class ConfirmationViewModel
    {
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