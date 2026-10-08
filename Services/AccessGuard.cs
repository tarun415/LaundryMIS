using Dapper;
using LaudaryMis.Helpers;
using LaudaryMis.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace LaudaryMis.Services
{
    public class AccessGuard : IAccessGuard
    {
        private readonly string _connStr;

        public AccessGuard(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<bool> CanAccessPickupAsync(ClaimsPrincipal user, int pickupId)
        {
            if (user.IsAdmin()) return true;

            return await CanSeeAsync(user, @"
                SELECT HospitalId, ProviderId
                FROM LaundryPickup
                WHERE PickupId = @id", pickupId);
        }

        public async Task<bool> CanAccessDeliveryAsync(ClaimsPrincipal user, int deliveryId)
        {
            if (user.IsAdmin()) return true;

            return await CanSeeAsync(user, @"
                SELECT LP.HospitalId, LP.ProviderId
                FROM DeliveryChallan DC
                INNER JOIN LaundryPickup LP ON LP.PickupId = DC.PickupId
                WHERE DC.DeliveryId = @id", deliveryId);
        }

        public async Task<bool> CanAccessAgreementAsync(ClaimsPrincipal user, int agreementId)
        {
            if (user.IsAdmin()) return true;

            return await CanSeeAsync(user, @"
                SELECT HospitalId, ProviderId
                FROM ProviderHospitalAgreements
                WHERE Id = @id", agreementId);
        }

        public async Task<RecordOwner?> GetAgreementOwnerAsync(int agreementId)
        {
            using var con = new SqlConnection(_connStr);

            return await con.QueryFirstOrDefaultAsync<RecordOwner>(@"
                SELECT HospitalId, ProviderId
                FROM ProviderHospitalAgreements
                WHERE Id = @agreementId", new { agreementId });
        }

        public async Task<bool> DeliveriesBelongToPickupAsync(int pickupId, IEnumerable<int> deliveryIds)
        {
            var ids = deliveryIds.Distinct().ToList();
            if (ids.Count == 0) return false;

            using var con = new SqlConnection(_connStr);

            int found = await con.ExecuteScalarAsync<int>(@"
                SELECT COUNT(DISTINCT DeliveryId)
                FROM DeliveryChallan
                WHERE PickupId = @pickupId AND DeliveryId IN @ids", new { pickupId, ids });

            return found == ids.Count;
        }

        public async Task<bool> DailyEntriesBelongToHospitalAsync(int hospitalId, IEnumerable<int> entryIds)
        {
            var ids = entryIds.Distinct().ToList();
            if (ids.Count == 0) return false;

            using var con = new SqlConnection(_connStr);

            int found = await con.ExecuteScalarAsync<int>(@"
                SELECT COUNT(DISTINCT Id)
                FROM DailyEntries
                WHERE HospitalId = @hospitalId AND Id IN @ids", new { hospitalId, ids });

            return found == ids.Count;
        }

        public async Task<HashSet<int>?> VisiblePickupIdsAsync(ClaimsPrincipal user)
        {
            if (user.IsAdmin()) return null;

            return await VisibleIdsAsync(user,
                "SELECT PickupId FROM LaundryPickup WHERE HospitalId = @id",
                "SELECT PickupId FROM LaundryPickup WHERE ProviderId = @id");
        }

        public async Task<HashSet<int>?> VisibleDeliveryIdsAsync(ClaimsPrincipal user)
        {
            if (user.IsAdmin()) return null;

            return await VisibleIdsAsync(user,
                @"SELECT DC.DeliveryId FROM DeliveryChallan DC
                  INNER JOIN LaundryPickup LP ON LP.PickupId = DC.PickupId
                  WHERE LP.HospitalId = @id",
                @"SELECT DC.DeliveryId FROM DeliveryChallan DC
                  INNER JOIN LaundryPickup LP ON LP.PickupId = DC.PickupId
                  WHERE LP.ProviderId = @id");
        }

        public async Task<HashSet<int>?> VisibleAgreementIdsAsync(ClaimsPrincipal user)
        {
            if (user.IsAdmin()) return null;

            return await VisibleIdsAsync(user,
                "SELECT Id FROM ProviderHospitalAgreements WHERE HospitalId = @id",
                "SELECT Id FROM ProviderHospitalAgreements WHERE ProviderId = @id");
        }

        // The query returns the hospital and vendor of one record
        private async Task<bool> CanSeeAsync(ClaimsPrincipal user, string sql, int id)
        {
            using var con = new SqlConnection(_connStr);

            var owner = await con.QueryFirstOrDefaultAsync<RecordOwner>(sql, new { id });

            return owner != null && user.CanSee(owner.HospitalId, owner.ProviderId);
        }

        private async Task<HashSet<int>> VisibleIdsAsync(ClaimsPrincipal user, string hospitalSql, string providerSql)
        {
            if (user.IsInRole("Hospital") && user.HospitalId() is int hospitalId)
                return await IdsAsync(hospitalSql, hospitalId);

            if (user.IsInRole("ServiceProvider") && user.ProviderId() is int providerId)
                return await IdsAsync(providerSql, providerId);

            return new HashSet<int>();
        }

        private async Task<HashSet<int>> IdsAsync(string sql, int id)
        {
            using var con = new SqlConnection(_connStr);

            return (await con.QueryAsync<int>(sql, new { id })).ToHashSet();
        }
    }
}
