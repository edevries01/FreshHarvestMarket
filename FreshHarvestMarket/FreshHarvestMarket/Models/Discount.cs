/*
 * Discount.cs
 * FreshHarvestMarket
 *
 * This class represents a discount applied to a specific produce item.
 *
 * It is used to support promotional pricing in the marketplace.
 *
 * It includes:
 * - A percentage-based discount amount (1%–99%)
 * - A required link to a Produce item via foreign key
 * - A navigation property to access the related Produce entity
 *
 * Data annotations are used to enforce validation rules such as required fields
 * & valid discount percentage ranges.
 */

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace FreshHarvestMarket.Models
{
    /// <summary>
    /// Represents a discount placed on a product
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// Primary key for Discount entity
        /// </summary>
        [Key]
        public int DiscountId { get; set; }

        /// <summary>
        /// A percent off for a discount
        /// </summary>
        [Required(ErrorMessage ="You must enter a discount amount.")]
        [Range(1, 99, ErrorMessage = "Enter a percent-off between 1% and 99%")]
        public int DiscountAmount { get; set; }

        /// <summary>
        /// Navigation property to produce item this discount is for
        /// </summary>
        [ValidateNever]
        public Produce? Produce { get; set; }

        /// <summary>
        /// Foreign key to produce item this discount is for
        /// </summary>
        [Required(ErrorMessage = "Please select valid produce")]
        public int ProduceId { get; set; }


    }
}
