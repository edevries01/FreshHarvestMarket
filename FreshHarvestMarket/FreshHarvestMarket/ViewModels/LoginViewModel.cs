/*
 * LoginViewModel.cs
 * FreshHarvestMarket
 *
 * This class is used to capture user input during the login process.
 *
 * It contains the necessary fields for authentication:
 * - Username & password for user sign-in
 * - RememberMe option for persistent login sessions
 * - ReturnUrl to redirect the user after successful login
 *
 * Data annotations are used to enforce validation rules such as
 * required fields & input length constraints.
 */

using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(255)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(255)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }

        public bool RememberMe { get; set; }

    }
}
