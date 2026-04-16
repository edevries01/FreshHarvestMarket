namespace FreshHarvestMarket.Models
{
    public class CartItem
    {
        public int ProduceId { get; set; }
        public string ProduceName { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public decimal LineTotal => Price * Quantity;
    }
}