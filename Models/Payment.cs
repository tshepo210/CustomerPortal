using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerPortal.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public SystemUser? User { get; set; }

        [Required]
        [Range(1, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("^(ZAR|USD|EUR|GBP)$")]
        public required string Currency { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z0-9]{8,11}$")]
        public required string SwiftCode { get; set; }

        [Required]
        public required string AccountNumber { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Verification / Submission fields
        public bool IsVerified { get; set; } = false;
        public string? VerifiedById { get; set; }

        [ForeignKey("VerifiedById")]
        [ValidateNever]
        public SystemUser? VerifiedBy { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public bool IsSubmitted { get; set; } = false;
        public DateTime? SubmittedAt { get; set; }
        public string? SwiftReference { get; set; }
    }
}
