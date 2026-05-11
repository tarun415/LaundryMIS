using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly IDbConnection _db;

        public DeliveryRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<VerifyDeliveryVM?> GetDeliveryByIdAsync(int deliveryId)
        {
            var sql = @"
                SELECT TOP 1
                    DeliveryId,
                    LinenType,
                    CleanCount,
                    VerifiedQty
                FROM DeliveryItems
                WHERE DeliveryId = @deliveryId";

            return await _db.QueryFirstOrDefaultAsync<VerifyDeliveryVM>(
                sql,
                new { deliveryId });
        }

        public async Task<int> VerifyDeliveryAsync(
            VerifyDeliveryModel model)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using var tran = _db.BeginTransaction();

            try
            {
                // 1️⃣ Update DeliveryItems
                var itemSql = @"
                    UPDATE DeliveryItems
                    SET VerifiedQty = @VerifiedQty
                    WHERE DeliveryId = @DeliveryId";

                await _db.ExecuteAsync(
                    itemSql,
                    new
                    {
                        model.VerifiedQty,
                        model.DeliveryId
                    },
                    tran);

                // 2️⃣ Update DeliveryEntries
                var entrySql = @"
                    UPDATE DeliveryEntries
                    SET 
                        LogBookPath = @LogBookPath,
                        VerifiedBy = @VerifiedBy,
                        VerifiedDate = GETDATE(),
                        IsVerified = 1,
                        VerificationRemark = @VerificationRemark
                    WHERE Id = @DeliveryId";

                var result = await _db.ExecuteAsync(
                    entrySql,
                    new
                    {
                        model.LogBookPath,
                        model.VerifiedBy,
                        model.VerificationRemark,
                        model.DeliveryId
                    },
                    tran);

                tran.Commit();

                return result;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public async Task<List<VerifyDeliveryVM>> GetPendingVerificationsAsync()
        {
            var sql = @"
                SELECT 
                    di.DeliveryId,
                    di.LinenType,
                    di.CleanCount,
                    di.VerifiedQty
                FROM DeliveryItems di
                INNER JOIN DeliveryEntries de
                    ON de.Id = di.DeliveryId
                WHERE ISNULL(de.IsVerified,0) = 0
                ORDER BY de.DeliveryDate DESC";

            var data = await _db.QueryAsync<VerifyDeliveryVM>(sql);

            return data.ToList();
        }
    }
}