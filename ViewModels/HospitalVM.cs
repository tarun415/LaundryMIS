using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaudaryMis.ViewModels
{
    using System.ComponentModel.DataAnnotations;


  
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class HospitalVM
    {
        public int? HospitalId { get; set; } = 0;

        [Required(ErrorMessage = "Hospital Name is required")]
        public string? HospitalName { get; set; }

        //[Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        public string? City { get; set; }

        [Required(ErrorMessage = "Contact Person is required")]
        public string? ContactPerson { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10 digit phone")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //[MinLength(4, ErrorMessage = "Password must be at least 4 characters")]
        public string? Password { get; set; }   

        //[Required(ErrorMessage = "State is required")]
        public int? StateId { get; set; }=0;

        [Required(ErrorMessage = "District is required")]
        public int? DistrictId { get; set; } = 0;  

        public string? DistrictName { get; set; }

        public List<SelectListItem> StateList { get; set; } = new();
        public List<SelectListItem> DistrictList { get; set; } = new();

        public bool IsActive { get; set; }=true;
    }
}
