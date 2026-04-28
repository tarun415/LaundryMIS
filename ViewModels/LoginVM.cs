namespace LaudaryMis.ViewModels
{
    public class LoginVM
    {
        public int RoleId { get; set; }

        public string? Username { get; set; }

        public int? DistrictId { get; set; }
        public int? HospitalId { get; set; }
        public int? ProviderId { get; set; }

        public string Password { get; set; } = string.Empty;
    }
}