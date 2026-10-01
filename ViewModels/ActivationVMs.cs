using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    public class ActivationRowVM
    {
        public string EntityType { get; set; } = string.Empty;   // Hospital | Provider
        public int EntityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? District { get; set; }
        public string Status { get; set; } = string.Empty;       // Registered | CodeIssued | NoCode
        public DateTime? CodeIssuedOn { get; set; }
        public DateTime? RegisteredOn { get; set; }
    }

    public class IssuedCodeVM
    {
        public int EntityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? District { get; set; }
        public string Code { get; set; } = string.Empty;
    }

    public class ActivationIndexVM
    {
        public string EntityType { get; set; } = "Hospital";
        public List<ActivationRowVM> Rows { get; set; } = new();
    }

    public class ActivationIssuedVM
    {
        public string EntityType { get; set; } = "Hospital";
        public List<IssuedCodeVM> Codes { get; set; } = new();
        public DateTime IssuedOn { get; set; } = DateTime.Now;
    }

    // Register as a hospital / vendor that is already on the tender list.
    public class ClaimVM
    {
        [Range(2, 3, ErrorMessage = "Select Hospital or Vendor.")]
        public int RoleId { get; set; } = 2;

        public int? DistrictId { get; set; }
        public int? HospitalId { get; set; }
        public int? ProviderId { get; set; }

        [Required(ErrorMessage = "Enter the activation code.")]
        [StringLength(30)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the mobile number.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter the email.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a password.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "The password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "The password must contain at least one letter and one number.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Re-enter the password.")]
        [Compare(nameof(Password), ErrorMessage = "The two passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ClaimResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public LaudaryMis.Models.User? User { get; set; }
        public bool BadCode { get; set; }     // counts towards the brute-force limit
    }

    public class NameOption
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
