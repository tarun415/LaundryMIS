using LaudaryMis.Models;
using LaudaryMis.Repositories;

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
    }
}
