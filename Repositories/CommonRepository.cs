using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.ViewModels;
using System.Data;
using static LaudaryMis.ViewModels.CommonVM;

namespace LaudaryMis.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _db;

        public CommonRepository(IConfiguration config, IDbConnection db)
        {
            _config = config;
            _db = db;
        }

        public async Task<List<DropdownVM>> GetWards()
        {
            var sql = @"SELECT WardId as Id, WardName  as Name
                        FROM tbl_Wards 
                        WHERE IsActive = 1";

            return (await _db.QueryAsync<DropdownVM>(sql)).ToList();
        }

        public async Task<List<LinenType>> GetLinenTypes()
        {
            var sql = @"SELECT LinenTypeId, LinenName 
                        FROM LinenType 
                        ORDER BY LinenTypeId";

            var data = await _db.QueryAsync<LinenType>(sql);

            return data.ToList();
        }

        public async Task<ProvidersVM> GetProviderByIdAsync(int id)
        {
            var sql = @"SELECT *
                        FROM Providers
                        WHERE ProviderId = @Id";

            return await _db.QueryFirstOrDefaultAsync<ProvidersVM>(
                sql,
                new { Id = id });
        }

        public async Task<List<DropdownVM>> GetHospitalsByProvider(int providerId)
        {
            var sql = @"select distinct hs.HospitalId as Id, hs.HospitalName as Name from ProviderHospitalAgreements as ag left join tbl_Hospitals hs on ag.ProviderId= hs.HospitalId where ag.ProviderId= = @ProviderId";

            var data = await _db.QueryAsync<DropdownVM>(
                sql,
                new { ProviderId = providerId });

            return data.ToList();
        }

        public async Task<List<DropdownVM>> GetProviderByHospital(int hospitalId)
        {
            var sql = @"select distinct pv.ProviderId as Id, pv.ProviderName as Name from ProviderHospitalAgreements as ag left join tbl_Providers pv on ag.ProviderId= pv.ProviderId where ag.HospitalId= @HospitalId";

            var data = await _db.QueryAsync<DropdownVM>(
                sql,
                new { HospitalId = hospitalId });

            return data.ToList();
        }

        public async Task<GetAgreementByHospitalVM> GetAgreementByHospital(int hospitalId)
{
    var sql = @"SELECT TOP 1
                    Id as AgreementId,
                    providerId,
                    StartDate,
                    EndDate
                FROM ProviderHospitalAgreements
                WHERE HospitalId = @HospitalId and IsActive=1
                ORDER BY Id DESC";

    return await _db.QueryFirstOrDefaultAsync<GetAgreementByHospitalVM>(
        sql,
        new { HospitalId = hospitalId });
}
    }
}