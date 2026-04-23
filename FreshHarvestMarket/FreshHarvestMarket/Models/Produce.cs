using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.Models
{
    public class Produce
    {
        public int ProduceId { get; set; }

        [Required(ErrorMessage = "Produce name is required")]
        public string ProduceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string? ProduceDescription { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000)]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string ProduceCategory { get; set; } = string.Empty;

        [Required(ErrorMessage = "Inventory is required")]
        [Range(0, int.MaxValue)]
        public int InventoryTotal { get; set; }

        public string? ImageUrl { get; set; }

        /// <summary>
        /// Navigation property for discount tied to produce item
        /// </summary>
        public Discount? Discount { get; set; }

    }
}
