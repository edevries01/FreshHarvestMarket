namespace FreshHarvestMarket.Models
{
    public class CartItem
    {
        public int ProduceId { get; set; }

        public Produce? Produce { get; set; }

        public string ProduceName { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public decimal LineTotal => Price * Quantity;

        public int? DiscountAmount { get; set; }  // Discount percentage, nullable because it won't always be applied
        public decimal DiscountedPrice => DiscountAmount.HasValue
                                            ? Price * (1 - DiscountAmount.Value / 100m)
                                            : Price;  // Calculate discounted price
    }
}