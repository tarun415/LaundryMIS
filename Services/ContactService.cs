using Dapper;
using LaudaryMis.Helpers;
using LaudaryMis.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace LaudaryMis.Services
{
    // Keeps made-up contact details out of hospital / vendor registrations. Nothing is sent to the
    // number or the address (no OTP), so this proves the details are plausible and unique, not who owns them.
    public class ContactService : IContactService
    {
        private readonly string _connStr;
        private readonly EmailDomainChecker _domains;

        public ContactService(IConfiguration config, EmailDomainChecker domains)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _domains = domains;
        }

        public async Task<ContactCheck> CheckAsync(string? phone, string? email, int? ownHospitalId = null, int? ownProviderId = null) =>
            new(await CheckMobileAsync(phone, ownHospitalId, ownProviderId), await CheckEmailAsync(email));

        // A mobile number can belong to one hospital / firm only, because it can be used to sign in
        public async Task<string?> CheckMobileAsync(string? phone, int? ownHospitalId = null, int? ownProviderId = null)
        {
            var problem = ContactRules.MobileProblem(phone);
            if (problem != null) return problem;

            using var con = new SqlConnection(_connStr);
            var taken = await con.ExecuteScalarAsync<int>(@"
                SELECT (SELECT COUNT(*) FROM tbl_Hospitals
                         WHERE Phone = @phone AND (@ownHospitalId IS NULL OR HospitalId <> @ownHospitalId))
                     + (SELECT COUNT(*) FROM tbl_Providers
                         WHERE Phone = @phone AND (@ownProviderId IS NULL OR ProviderId <> @ownProviderId))",
                new { phone = phone!.Trim(), ownHospitalId, ownProviderId });

            return taken > 0
                ? "This mobile number is already registered for another hospital / firm. Use your own number, or sign in."
                : null;
        }

        private async Task<string?> CheckEmailAsync(string? email)
        {
            var problem = ContactRules.EmailProblem(email);
            if (problem != null) return problem;

            var accepts = await _domains.AcceptsMailAsync(ContactRules.DomainOf(email!));
            return accepts == false
                ? "This email domain does not exist or cannot receive mail. Check the spelling (for example name@gmail.com)."
                : null;
        }
    }
}
