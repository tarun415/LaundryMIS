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

        public string Password { get; set; } = string.Empty;
    }
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public User? User { get; set; }
    }
}