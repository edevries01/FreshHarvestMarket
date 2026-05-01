/*
 * Produce.cs
 * FreshHarvestMarket
 *
 * This class represents a produce item available in the marketplace.
 *
 * It includes details such as:
 * - Name & description of the item
 * - Price per unit
 * - Category (fruit, vegetable, other)
 * - Inventory quantity available
 * - Optional image reference
 *
 * Data annotations are used to enforce validation rules for required
 * fields, value ranges, & input constraints.
 *
 * This model also includes a navigation property to an optional
 * Discount, allowing discounted pricing to be applied when available.
 */

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
