namespace LaudaryMis.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string? Username { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string? RoleName { get; set; }

        public int? HospitalId { get; set; }

        public int? ProviderId { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }
    }
}