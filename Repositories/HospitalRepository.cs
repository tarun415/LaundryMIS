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

    //public async Task InsertAsync(HospitalVM model)
    //{
    //    if (_db.State == ConnectionState.Closed)
    //        _db.Open();

    //    await _db.ExecuteAsync(@"
    //        INSERT INTO tbl_Hospitals
    //        (HospitalName, Address, City, ContactPerson, Phone, Email, IsActive)
    //        VALUES
    //        (@HospitalName, @Address, @City, @ContactPerson, @Phone, @Email, @IsActive)
    //    ", model);
    //}

    public async Task<IEnumerable<HospitalVM>> GetAllAsync()
    {
        return await _db.QueryAsync<HospitalVM>("SELECT HospitalId , HospitalName , Address , City , ContactPerson , Phone , Email, isnull(IsActive,0) [IsActive], CreatedOn , HospitalCode  FROM tbl_Hospitals order by HospitalId desc ");
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
    public async Task UpdateAsync(HospitalVM model)
    {
        if (_db.State == ConnectionState.Closed)
            _db.Open();

        await _db.ExecuteAsync(@"
        UPDATE tbl_Hospitals
        SET 
            HospitalName = @HospitalName,
            Address = @Address,
            City = @City,
            IsActive= @IsActive,
 ContactPerson = @ContactPerson,
            Phone = @Phone,
            Email= @Email
        WHERE HospitalId = @HospitalId
    ", model);
    }


    public async Task SaveAsync(HospitalVM model)
    {
        if (model.HospitalId == 0)
            await InsertAsync(model);
        else
            await UpdateAsync(model);
    }


    public async Task<HospitalVM> GetHospitalByIdAsync(int id)
    {
        return await _db.QueryFirstOrDefaultAsync<HospitalVM>(@"
        SELECT HospitalId , HospitalName , Address , City , ContactPerson , Phone , Email, isnull(IsActive,0) [IsActive], CreatedOn , HospitalCode  FROM tbl_Hospitals
        WHERE HospitalId = @Id
    ", new { Id = id });
    }
    public async Task DeleteAsync(int id)
    {
        if (_db.State == ConnectionState.Closed)
            _db.Open();

        await _db.ExecuteAsync(@"
        UPDATE tbl_Hospitals
        SET IsActive = 0
        WHERE HospitalId = @Id
    ", new { Id = id });
    }
}