using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IWardService
    {
        Task<IEnumerable<WardVM>> GetWardAsync();
        Task<WardVM> GetWardByIdAsync(int id);
        Task SaveAsync(WardVM model);
        Task<bool> DeleteAsync(int id);
    }
}
