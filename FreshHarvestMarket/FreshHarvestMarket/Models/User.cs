/*
 * User.cs
 * FreshHarvestMarket
 *
 * This class extends the ASP.NET Core IdentityUser to represent
 * application users within the system.
 *
 * It includes additional optional profile fields such as first name
 * & last name, beyond the default Identity properties.
 *
 * This model is used for authentication, authorization, & storing
 * user-related data within the application.
 */

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.Models
{
    public class User : IdentityUser
    { 
        // Not required
        public string? FirstName { get; set; }

        // Not required
        public string? LastName { get; set; }
    }
}
