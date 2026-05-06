using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IProviderService
    {
        Task<IEnumerable<Provider>> GetAll();

        Task SaveAsync(ProvidersVM model);
        Task CreateProviderWithLogin(ProvidersVM model);
        Task<IEnumerable<ProvidersVM>> GetProviderAsync();
        Task<ProvidersVM> GetProviderByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
      
    }
}
