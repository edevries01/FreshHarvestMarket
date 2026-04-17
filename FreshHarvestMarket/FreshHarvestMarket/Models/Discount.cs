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
