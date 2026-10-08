using LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class LoginVM
    {
        public int RoleId { get; set; }

        // Admin signs in with a username
        public string? Username { get; set; }

        // Hospital and vendor sign in with the mobile number or the email they registered with
        public string? LoginId { get; set; }

        // CMS signs in by choosing the district and the hospital it belongs to
        public int? DistrictId { get; set; }
        public int? HospitalId { get; set; }

        public string Password { get; set; } = string.Empty;
    }
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public User? User { get; set; }
    }

    public class ChangePasswordVM
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Enter your current password.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Enter a new password.")]
        [System.ComponentModel.DataAnnotations.MinLength(8, ErrorMessage = "The new password must be at least 8 characters.")]
        public string NewPassword { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Confirm the new password.")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(NewPassword), ErrorMessage = "The two passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
