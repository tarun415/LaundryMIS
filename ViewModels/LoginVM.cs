using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    public class LoginVM
    {
        //[Required]
        //public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public string? RoleName { get; set; }
    }
}
