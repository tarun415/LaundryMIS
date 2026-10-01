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
        public const string SafeNameMessage = "Naam mein < > \" ' ` & jaise characters allowed nahi hain.";

        [Range(2, 3, ErrorMessage = "Hospital ya Vendor chunein.")]
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
        [Required(ErrorMessage = "Mobile number daalein.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "10 digit ka sahi mobile number daalein.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email daalein.")]
        [EmailAddress(ErrorMessage = "Sahi email daalein.")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password daalein.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password kam se kam 8 characters ka hona chahiye.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "Password mein kam se kam ek letter aur ek number hona chahiye.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password dobara daalein.")]
        [Compare(nameof(Password), ErrorMessage = "Dono password match nahi kar rahe.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
