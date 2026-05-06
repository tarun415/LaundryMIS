using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IWardRepository
    {
        Task<IEnumerable<WardVM>> GetWardAsync();
        Task<WardVM> GetWardByIdAsync(int id);
        Task SaveAsync(WardVM model);
        Task<bool> DeleteAsync(int id);
        Task<List<Ward>> GetWardNamesAsync();
    }
}
