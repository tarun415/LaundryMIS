using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IProviderRepository
    {
        Task DeleteAsync(int id);
        Task<IEnumerable<Provider>> GetAll();
        Task<IEnumerable<ProvidersVM>> GetProviderAsync();
        Task<ProvidersVM> GetProviderByIdAsync(int id);
        Task InsertAsync(ProvidersVM model);
        Task SaveAsync(ProvidersVM model);
        Task UpdateAsync(ProvidersVM model);
    }
}
