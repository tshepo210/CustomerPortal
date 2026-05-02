using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
