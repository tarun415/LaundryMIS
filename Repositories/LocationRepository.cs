using Dapper;
using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using System.Data;

public class LocationRepository : ILocationRepository
{
    private readonly IDbConnection _db;

    public LocationRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<List<State>> GetStates()
    {
        var data = await _db.QueryAsync<State>(
            "SELECT StateId, StateName FROM StateMaster ORDER BY StateName");

        return data.ToList();
    }

    public async Task<List<District>> GetDistrictsByState(int stateId)
    {
        var data = await _db.QueryAsync<District>(
            "SELECT DistrictId, DistrictName FROM DistrictMaster WHERE StateId=@stateId ORDER BY DistrictName",
            new { stateId });

        return data.ToList();
    }
}