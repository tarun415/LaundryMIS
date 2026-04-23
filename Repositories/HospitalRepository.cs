using Dapper;
using LaudaryMis.ViewModels;
using System.Data;

public class HospitalRepository
{
    private readonly IDbConnection _db;

    public HospitalRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task InsertAsync(HospitalVM model)
    {
        if (_db.State == ConnectionState.Closed)
            _db.Open();

        await _db.ExecuteAsync(@"
            INSERT INTO tbl_Hospitals
            (HospitalName, Address, City, ContactPerson, Phone, Email, IsActive)
            VALUES
            (@HospitalName, @Address, @City, @ContactPerson, @Phone, @Email, @IsActive)
        ", model);
    }

    public async Task<IEnumerable<HospitalVM>> GetAllAsync()
    {
        return await _db.QueryAsync<HospitalVM>("SELECT * FROM tbl_Hospitals");
    }
}