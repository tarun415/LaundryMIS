using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
   

    public class ProvidersVM
    {
        public int ProviderId { get; set; } = 0;

        [Required(ErrorMessage = "Provider Name is required.")]
        public string? ProviderName { get; set; }

      //  [Required(ErrorMessage = "Firm Name is required.")]
        public string? FirmName { get; set; }

        [Required(ErrorMessage = "Rate Per Bed is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Rate Per Bed must be a valid number.")]
        public int? RatePerBed { get; set; }

       // [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

       // [Required(ErrorMessage = "Phone is required.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
