using LaudaryMis.Models;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<State>> GetStates();
        Task<List<District>> GetDistrictsByState(int stateId);
    }
}