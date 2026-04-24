using LaudaryMis.Models;
using LaudaryMis.Repositories;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class ProviderService
    {
        private readonly ProviderRepository _repo;

        public ProviderService(ProviderRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task SaveAsync(ProvidersVM model)
        {
            if (string.IsNullOrWhiteSpace(model.ProviderName))
                throw new Exception("Provider name required");

            if (model.ProviderId == 0)
            {
                await _repo.InsertAsync(model);   // New
            }
            else
            {
                await _repo.UpdateAsync(model);   // Edit
            }
        }
        public async Task<IEnumerable<ProvidersVM>> GetProviderAsync()
        {
            return await _repo.GetProviderAsync();
        }
        public async Task<ProvidersVM> GetProviderByIdAsync(int id)
        {
            return await _repo.GetProviderByIdAsync(id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _repo.GetProviderByIdAsync(id);

            if (data == null)
                return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
