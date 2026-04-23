namespace LaudaryMis.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int ProviderId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public int RoleId { get; set; }
        public int? HospitalId { get; set; }
        public bool IsActive { get; set; }
        public string? RoleName { get; set; } // 🔥 add this

    }
}
