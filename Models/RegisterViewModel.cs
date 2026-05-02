using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]{2,50}$",
            ErrorMessage = "Name must contain only letters and spaces (2–50 characters).")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [RegularExpression(@"^[A-Z]{3}$",
            ErrorMessage = "Country must be a 3-letter ISO code (e.g. ZAF, USA).")]
        public required string Country { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,40}$",
            ErrorMessage = "Password must be 8–40 characters and include uppercase, number, and special character.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}