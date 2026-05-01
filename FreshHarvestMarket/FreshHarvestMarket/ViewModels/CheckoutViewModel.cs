/*
 * CheckoutViewModel
 * FreshHarvestMarket
 *
 * This ViewModel is used to collect customer information during the checkout process.
 *
 * It captures the necessary details required to place an order, including:
 * - First & last name
 * - Email address
 * - Phone number
 * - Preferred pickup date
 *
 * Data annotations are used to validate user input, ensuring:
 * - Required fields are completed
 * - Proper formatting for names, email, & phone number
 * - Pickup date is provided
 */

using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First name must contain letters only")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last name must contain letters only")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone must be 10 digits only")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "Pickup date is required")]
        public DateTime PickupDate { get; set; }
    }
}