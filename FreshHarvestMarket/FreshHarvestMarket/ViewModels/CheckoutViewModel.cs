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