using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models
{
    public class CreateUserViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
