using LaudaryMis.Models;
using LaudaryMis.Repositories;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;

namespace LaudaryMis.Services
{
    public class ProviderService: IProviderService
    {
        private readonly IProviderRepository _repo;
        public ProviderService(IProviderRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Provider>> GetAll()
        {
            return await _repo.GetAll();
        }
    }
}
