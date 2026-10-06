namespace LaudaryMis.Services.Interfaces
{
    // PhoneError / EmailError are shown next to the field; null means that value is fine.
    public record ContactCheck(string? PhoneError, string? EmailError)
    {
        public bool Ok => PhoneError == null && EmailError == null;
    }

    public interface IContactService
    {
        // Mobile number and email of a hospital / vendor. ownHospitalId / ownProviderId is the record the
        // contact belongs to (when it already exists), so its own number is not counted as "taken".
        Task<ContactCheck> CheckAsync(string? phone, string? email, int? ownHospitalId = null, int? ownProviderId = null);

        Task<string?> CheckMobileAsync(string? phone, int? ownHospitalId = null, int? ownProviderId = null);
    }
}
