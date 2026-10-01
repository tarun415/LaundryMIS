using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    // Self-registration for Hospitals (RoleId 2) and Providers / Vendors (RoleId 3).
    // Role-specific required fields are checked in AccountController.Register.
    public class RegisterVM
    {
        // Names show up in dropdowns and pages for other users, so HTML
        // special characters are not allowed.
        public const string SafeNamePattern = @"^[^<>""'`&]*$";
        public const string SafeNameMessage = "The name may not contain characters such as < > \" ' ` &.";

        [Range(2, 3, ErrorMessage = "Select Hospital or Vendor.")]
        public int RoleId { get; set; } = 2;

        // ── Hospital ──────────────────────────────────────────
        [StringLength(200)]
        [RegularExpression(SafeNamePattern, ErrorMessage = SafeNameMessage)]
        public string? HospitalName { get; set; }

        public int? DistrictId { get; set; }

        [StringLength(100)]
        [RegularExpression(SafeNamePattern, ErrorMessage = SafeNameMessage)]
        public string? ContactPerson { get; set; }

        [StringLength(250)]
        [RegularExpression(SafeNamePattern, ErrorMessage = SafeNameMessage)]
        public string? Address { get; set; }

        // ── Provider / Vendor ─────────────────────────────────
        [StringLength(150)]
        [RegularExpression(SafeNamePattern, ErrorMessage = SafeNameMessage)]
        public string? ProviderName { get; set; }

        [StringLength(150)]
        [RegularExpression(SafeNamePattern, ErrorMessage = SafeNameMessage)]
        public string? FirmName { get; set; }

        // ── Common ────────────────────────────────────────────
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
}
