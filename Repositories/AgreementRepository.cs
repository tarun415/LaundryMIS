using Dapper;
using LaudaryMis.ViewModels;
using System.Data;
using LaudaryMis.Repositories.Interfaces;

public class AgreementRepository : IAgreementRepository
{
    private readonly IDbConnection _db;

    public AgreementRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task InsertAsync(AgreementVM model, string? filePath)
    {
        if (_db.State == ConnectionState.Closed)
            _db.Open();

        // deactivate old
        await _db.ExecuteAsync(@"
            UPDATE ProviderHospitalAgreements
            SET IsActive = 0
            WHERE ProviderId = @ProviderId
            AND HospitalId = @HospitalId
            AND IsActive = 1
        ", model);

        // insert new
        await _db.ExecuteAsync(@"
            INSERT INTO ProviderHospitalAgreements
            (ProviderId, HospitalId, BedCount, RatePerBed, StartDate, EndDate, AgreementFile, IsActive)
            VALUES
            (@ProviderId, @HospitalId, @BedCount, @RatePerBed, @StartDate, @EndDate, @AgreementFile, 1)
        ", new
        {
            model.ProviderId,
            model.HospitalId,
            model.BedCount,
            model.RatePerBed,
            model.StartDate,
            model.EndDate,
            AgreementFile = filePath
        });
    }


    public async Task<IEnumerable<AgreementVM>> GetAllAsync()
    {
        var query = @"
SELECT 
    a.Id,
    a.ProviderId,
    a.HospitalId,
    a.BedCount,
    a.RatePerBed,
    a.StartDate,
    a.EndDate,
    a.AgreementFile AS FilePath,
    p.ProviderName,
    h.HospitalName
FROM ProviderHospitalAgreements a
LEFT JOIN tbl_Providers p ON a.ProviderId = p.ProviderId
LEFT JOIN tbl_Hospitals h ON a.HospitalId = h.HospitalId
WHERE a.IsActive = 1
";

        return await _db.QueryAsync<AgreementVM>(query);
    }
    public async Task<AgreementVM> GetByIdAsync(int id)
    {
        var query = "SELECT   Id,  ProviderId,  HospitalId,  BedCount,  RatePerBed,  StartDate,  EndDate,  AgreementFile AS FilePath FROM ProviderHospitalAgreements WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<AgreementVM>(query, new { Id = id });
    }
    public async Task DeleteAsync(int id)
    {
        var query = "DELETE FROM ProviderHospitalAgreements WHERE Id = @Id";
        await _db.ExecuteAsync(query, new { Id = id });
    }
    public async Task SaveAsync(AgreementVM model, string? filePath)
    {
        if (_db.State == ConnectionState.Closed)
            _db.Open();

        if (model.Id == 0)
        {
            //  INSERT
            await _db.ExecuteAsync(@"
            INSERT INTO ProviderHospitalAgreements
            (ProviderId, HospitalId, BedCount, RatePerBed, StartDate, EndDate, AgreementFile, IsActive)
            VALUES
            (@ProviderId, @HospitalId, @BedCount, @RatePerBed, @StartDate, @EndDate, @AgreementFile, 1)
        ", new
            {
                model.ProviderId,
                model.HospitalId,
                model.BedCount,
                model.RatePerBed,
                model.StartDate,
                model.EndDate,
                AgreementFile = filePath
            });
        }
        else
        {
            //  UPDATE
            await _db.ExecuteAsync(@"
            UPDATE ProviderHospitalAgreements SET
                ProviderId = @ProviderId,
                HospitalId = @HospitalId,
                BedCount = @BedCount,
                RatePerBed = @RatePerBed,
                StartDate = @StartDate,
                EndDate = @EndDate,
                AgreementFile = @AgreementFile
            WHERE Id = @Id
        ", new
            {
                model.Id,
                model.ProviderId,
                model.HospitalId,
                model.BedCount,
                model.RatePerBed,
                model.StartDate,
                model.EndDate,
                AgreementFile = filePath
            });
        }
    }
}