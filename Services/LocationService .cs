using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _repo;

    public LocationService(ILocationRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<State>> GetStates()
    {
        return await _repo.GetStates();
    }

    public async Task<List<District>> GetDistrictsByState(int stateId)
    {
        return await _repo.GetDistrictsByState(stateId);
    }
}