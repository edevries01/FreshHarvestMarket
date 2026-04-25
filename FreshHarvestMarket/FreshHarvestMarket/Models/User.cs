using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.Models
{
    public class User : IdentityUser
    {
        //Not required
        public string? FirstName { get; set; }

        //Not required
        public string? LastName { get; set; }
    }
}
