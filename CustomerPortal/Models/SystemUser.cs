using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CustomerPortal.Models
{
    public class SystemUser : IdentityUser
    {
        [RegularExpression(@"^[a-zA-Z\s]{2,50}$")]
        public required string FullName { get; set; }
        
        [RegularExpression(@"^[A-Z]{3}$")]
        public required string Country { get; set; }
    }
}
