using System.Security.Claims;

namespace LaudaryMis.Helpers
{
    // Who is signed in and which hospital / vendor they belong to, read from the login claims.
    public static class UserScope
    {
        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole("Admin");

        public static int? HospitalId(this ClaimsPrincipal user) => ClaimId(user, "HospitalId");

        public static int? ProviderId(this ClaimsPrincipal user) => ClaimId(user, "ProviderId");

        // Admin sees everything, a hospital only its own records and a vendor only the records of its own
        // contracts. Any other role, or a login without the id claim, sees nothing.
        public static bool CanSee(this ClaimsPrincipal user, int hospitalId, int providerId)
        {
            if (user.IsAdmin()) return true;

            if (user.IsInRole("Hospital"))
                return hospitalId > 0 && user.HospitalId() == hospitalId;

            if (user.IsInRole("Provider"))
                return providerId > 0 && user.ProviderId() == providerId;

            return false;
        }

        private static int? ClaimId(ClaimsPrincipal user, string type) =>
            int.TryParse(user.FindFirst(type)?.Value, out var id) && id > 0 ? id : null;
    }
}
