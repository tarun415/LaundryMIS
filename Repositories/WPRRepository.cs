using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using System.Data;

namespace LaudaryMis.Repositories
{
    public class WPRRepository: IWPRRepository
    {
        private readonly IDbConnection _db;

        public WPRRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> InsertAsync(WPRVM model, int hospitalId)
        {
            if (_db.State == ConnectionState.Closed)
                _db.Open();

            using var tx = _db.BeginTransaction();

            try
            {
                // ✅ DUPLICATE CHECK (NEW LOGIC)
                var exists = await _db.ExecuteScalarAsync<int>(@"
            SELECT COUNT(*) FROM WPREntries
            WHERE AgreementId = @AgreementId
            AND Week = @Week
            AND Month = @Month
            AND Year = @Year
        ", new
                {
                    model.AgreementId,
                    model.Week,
                    model.Month,
                    model.Year
                }, tx);

                if (exists > 0)
                    throw new Exception("WPR already exists for this week");

                // ✅ SCORE VALIDATION
                foreach (var d in model.Details)
                {
                    if (d.Score < 0 || d.Score > 10)
                        throw new Exception("Score must be between 0–10");
                }

                var total = model.Details.Sum(x => x.Score);

                // ✅ INSERT + RETURN ID
                var wprId = await _db.ExecuteScalarAsync<int>(@"
            INSERT INTO WPREntries
            (AgreementId, HospitalId, Week, Month, Year, StaffName, Remarks, TotalScore, Status)
            VALUES
            (@AgreementId, @HospitalId, @Week, @Month, @Year, @StaffName, @Remarks, @TotalScore, 'Submitted');

            SELECT CAST(SCOPE_IDENTITY() as int);
        ",
                new
                {
                    model.AgreementId,
                    HospitalId = hospitalId,
                    model.Week,
                    model.Month,
                    model.Year,
                    model.StaffName,
                    model.Remarks,
                    TotalScore = total
                }, tx);

                // ✅ INSERT DETAILS
                foreach (var d in model.Details)
                {
                    await _db.ExecuteAsync(@"
                INSERT INTO WPRDetails
                (WPRId, ParameterId, Score, Remarks)
                VALUES
                (@WPRId, @ParameterId, @Score, @Remarks)
            ",
                    new
                    {
                        WPRId = wprId,
                        d.ParameterId,
                        d.Score,
                        d.Remarks
                    }, tx);
                }

                tx.Commit();
                return wprId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
        public async Task<IEnumerable<WPRParameter>> GetParameters()
        {
            return await _db.QueryAsync<WPRParameter>(
                "SELECT * FROM WPRParameters WHERE IsActive = 1"
            );
        }

        public async Task<IEnumerable<AgreementVM>> GetHospitalAgreements(int hospitalId)
        {
            return await _db.QueryAsync<AgreementVM>(@"
        SELECT a.Id, h.HospitalName
        FROM ProviderHospitalAgreements a
        JOIN tbl_Hospitals h ON a.HospitalId = h.HospitalId
        WHERE a.HospitalId = @hospitalId AND a.IsActive = 1
    ", new { hospitalId });
        }
    }
}
