namespace FreshHarvestMarket.Models
{
    public class Produce
    {
        public int ProduceId { get; set; }

        public string ProduceName { get; set; } = string.Empty;

        public string? ProduceDescription { get; set; }

        public decimal UnitPrice { get; set; }

        public string ProduceCategory { get; set; } = string.Empty;

        public int InventoryTotal { get; set; }

        public string? ImageUrl { get; set; } = "placeholder.jpg";

        /// <summary>
        /// Navigation property for discount tied to produce item
        /// </summary>
        public Discount? Discount { get; set; }

    }
}
