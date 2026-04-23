using Dapper;
using LaudaryMis.ViewModels;
using System.Data;

public class AgreementRepository
{
    private readonly IDbConnection _db;

    public AgreementRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task InsertAsync(AgreementVM model, string filePath)
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
}