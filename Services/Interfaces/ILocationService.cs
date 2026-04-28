using LaudaryMis.Models;

namespace LaudaryMis.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<State>> GetStates();
        Task<List<District>> GetDistrictsByState(int stateId);
    }
}