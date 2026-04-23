

using Dapper;
using System.Data;
using LaudaryMis.Models;

namespace LaudaryMis.Repositories
{
    public class ProviderRepository
    {
        private readonly IDbConnection _db;

        public ProviderRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            return await _db.QueryAsync<Provider>(
                "SELECT * FROM tbl_Providers WHERE IsActive = 1"
            );
        }
    }

}