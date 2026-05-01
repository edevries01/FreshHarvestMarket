/*
 * RegisterViewModel.cs
 * FreshHarvestMarket
 *
 * This ViewModel is used for user registration input in the application.
 *
 * It collects & validates user-provided data required to create
 * a new account, including:
 * - Username, first name, & last name
 * - Email address
 * - Password & password confirmation
 *
 * Data annotations are used to enforce validation rules such as:
 * - Required fields
 * - String length constraints
 * - Email format validation
 * - Password confirmation matching
 *
 * This class is used in the Account/Register view & is part of the
 * ASP.NET Core Identity registration workflow.
 */

using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(255)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(255)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(255)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
