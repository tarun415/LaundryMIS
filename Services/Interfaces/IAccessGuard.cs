using System.Security.Claims;

namespace LaudaryMis.Services.Interfaces
{
    public record RecordOwner(int HospitalId, int ProviderId);

    // Answers "may this signed-in user touch this record?" for records that are opened by id. Admin may
    // touch everything; a hospital only its own pickups and deliveries; a vendor only those of its own
    // contracts. A record that does not exist is never accessible.
    public interface IAccessGuard
    {
        Task<bool> CanAccessPickupAsync(ClaimsPrincipal user, int pickupId);

        Task<bool> CanAccessDeliveryAsync(ClaimsPrincipal user, int deliveryId);

        Task<bool> CanAccessAgreementAsync(ClaimsPrincipal user, int agreementId);

        // Hospital and vendor of an agreement, or null when there is no such agreement
        Task<RecordOwner?> GetAgreementOwnerAsync(int agreementId);

        // True when every id is a delivery of that pickup
        Task<bool> DeliveriesBelongToPickupAsync(int pickupId, IEnumerable<int> deliveryIds);

        // True when every id is a daily entry of that hospital
        Task<bool> DailyEntriesBelongToHospitalAsync(int hospitalId, IEnumerable<int> entryIds);

        // For filtering lists: null means no restriction (admin), otherwise the ids the user may see
        Task<HashSet<int>?> VisiblePickupIdsAsync(ClaimsPrincipal user);

        Task<HashSet<int>?> VisibleDeliveryIdsAsync(ClaimsPrincipal user);

        // Agreements (also expired ones) the user is a party to, for lists that carry only an AgreementId
        Task<HashSet<int>?> VisibleAgreementIdsAsync(ClaimsPrincipal user);
    }
}
