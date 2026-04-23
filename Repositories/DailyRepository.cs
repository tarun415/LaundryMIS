using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class DailyRepository : IDailyRepository
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _db;

        public DailyRepository(IConfiguration config, IDbConnection db)
        {
            _config = config;
            _db = db;

        }

        public async Task<int> InsertAsync(DailyEntryVM model)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using var transaction = _db.BeginTransaction();

            try
            {
                var entryId = await _db.ExecuteScalarAsync<int>(@"
INSERT INTO DailyEntries
(EntryDate, Ward, Shift, ProviderId, HospitalId, CollectedBy, ReceivedBy, Supervisor, IsInfected, Remarks)
VALUES
(@EntryDate, @Ward, @Shift, @ProviderId, @HospitalId, @CollectedBy, @ReceivedBy, @Supervisor, @IsInfected, @Remarks);

SELECT CAST(SCOPE_IDENTITY() as int);
", new
                {
                    model.EntryDate,
                    model.Ward,
                    model.Shift,
                    model.ProviderId,
                    model.HospitalId,
                    model.CollectedBy,
                    model.ReceivedBy,
                    model.Supervisor,
                    IsInfected = model.IsInfected,
                    Remarks = model.Remarks ?? ""
                }, transaction);

                foreach (var item in model.Items)
                {
                    await _db.ExecuteAsync(@"
                    INSERT INTO DailyEntryItems (EntryId, LinenType, DirtyCount)
                    VALUES (@EntryId, @LinenType, @DirtyCount );",
                    new
                    {
                        EntryId = entryId,
                        item.LinenType,
                        item.DirtyCount,
                    }, transaction);
                }

                transaction.Commit();
                return entryId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<Hospital>> GetHospitalsByProvider(int providerId)
        {
            var sql = @"
        SELECT h.HospitalId, h.HospitalName
        FROM ProviderHospitals ph
        JOIN tbl_Hospitals h ON ph.HospitalId = h.HospitalId
        WHERE ph.ProviderId = @ProviderId";

            return (await _db.QueryAsync<Hospital>(sql, new { ProviderId = providerId })).ToList();
        }

        public async Task<List<DailyEntryVM>> GetAllEntries()
        {
            var sql = @"SELECT * FROM DailyEntries ORDER BY Id DESC";
            return (await _db.QueryAsync<DailyEntryVM>(sql)).ToList();
        }


        public async Task UpdateStatus(int id, string status)
        {
            var sql = "UPDATE DailyEntries SET Status=@status WHERE Id=@id";
            await _db.ExecuteAsync(sql, new { id, status });
        }

        public async Task<List<LinenType>> GetLinenTypes()
        {
            var sql = "SELECT LinenTypeId, LinenName FROM LinenType";

            var data = await _db.QueryAsync<LinenType>(sql);

            return data.ToList();
        }

        public async Task<IEnumerable<dynamic>> GetPendingEntries(int providerId)
        {
            return await _db.QueryAsync(@"
        SELECT 
            d.Id AS EntryId,
            d.EntryDate,
            h.HospitalName,    
            d.Status
        FROM DailyEntries d
        JOIN tbl_Hospitals h ON d.HospitalId = h.HospitalId
        WHERE d.ProviderId = @ProviderId
        AND d.Status IN ('Collected','Partial')", new { ProviderId = providerId });
        }

        public async Task<dynamic> GetEntryWithItems(int id)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            var entry = await _db.QueryFirstOrDefaultAsync(@"
        SELECT * FROM DailyEntries WHERE Id = @Id
    ", new { Id = id });

            // 🔥 HANDLE NULL
            if (entry == null)
                throw new Exception("Entry not found");

            var items = await _db.QueryAsync(@" SELECT di.LinenType,di.DirtyCount,ISNULL(SUM(dli.CleanCount), 0) AS DeliveredCount
    FROM DailyEntryItems di LEFT JOIN DeliveryEntries de ON de.EntryId = di.EntryId LEFT JOIN DeliveryItems dli ON dli.DeliveryId = de.Id 
        AND dli.LinenType = di.LinenType WHERE di.EntryId = @Id GROUP BY di.LinenType, di.DirtyCount", new { Id = id });

            return new
            {
                Entry = entry,
                Items = items.ToList()
            };
        }

        public async Task<int> InsertDelivery(DeliveryVM model)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using var transaction = _db.BeginTransaction();

            try
            {
                // ✅ Insert Delivery Entry
                var deliveryId = await _db.ExecuteScalarAsync<int>(@"
            INSERT INTO DeliveryEntries 
            (EntryId, DeliveredBy, ReceivedBackBy, Remarks)
            VALUES 
            (@EntryId, @DeliveredBy, @ReceivedBackBy, @Remarks);

            SELECT CAST(SCOPE_IDENTITY() as int);
        ", new
                {
                    model.EntryId,
                    model.DeliveredBy,
                    model.ReceivedBackBy,
                    Remarks = model.Remarks ?? ""
                }, transaction);

                // ✅ Insert Items
                foreach (var item in model.Items)
                {
                    if (item.CleanCount <= 0) continue;

                    await _db.ExecuteAsync(@"
                INSERT INTO DeliveryItems
                (DeliveryId, LinenType, CleanCount)
                VALUES
                (@DeliveryId, @LinenType, @CleanCount)
            ", new
                    {
                        DeliveryId = deliveryId,
                        item.LinenType,
                        item.CleanCount
                    }, transaction);
                }

                // 🔥 STEP 2 START — CALCULATE STATUS

                // Total Dirty
                var totalDirty = await _db.ExecuteScalarAsync<int>(@"
            SELECT ISNULL(SUM(DirtyCount),0)
            FROM DailyEntryItems
            WHERE EntryId = @EntryId
        ", new { model.EntryId }, transaction);

                // Total Delivered (ALL deliveries)
                var totalDelivered = await _db.ExecuteScalarAsync<int>(@"
            SELECT ISNULL(SUM(di.CleanCount),0)
            FROM DeliveryItems di
            INNER JOIN DeliveryEntries de ON di.DeliveryId = de.Id
            WHERE de.EntryId = @EntryId
        ", new { model.EntryId }, transaction);

                string status;

                if (totalDelivered == 0)
                    status = "Collected";
                else if (totalDelivered < totalDirty)
                    status = "Partial";
                else
                    status = "Delivered";

                // ✅ Update Correct Status
                await _db.ExecuteAsync(@"
            UPDATE DailyEntries
            SET Status = @Status
            WHERE Id = @EntryId
        ", new { Status = status, model.EntryId }, transaction);

                // 🔥 STEP 2 END

                transaction.Commit();
                return deliveryId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}