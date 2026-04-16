using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.Models
{
    //User Info
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer"; //Default Role

        //Property to display FullName in UI
        public string FullName => $"{FirstName} {LastName}";

        //Navigation property to navigate through orders
        public ICollection<Order>? Orders { get; set; }
    }
}
