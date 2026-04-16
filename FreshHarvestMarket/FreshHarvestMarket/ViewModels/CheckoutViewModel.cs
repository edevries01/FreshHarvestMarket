using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string Phone { get; set; } = "";

        [Required]
        public DateTime PickupDate { get; set; }
    }
}