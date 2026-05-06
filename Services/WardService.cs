using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class WardService : IWardService
    {
        private readonly IWardRepository _repo;

        public WardService(IWardRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<WardVM>> GetWardAsync()
        {
            return await _repo.GetWardAsync();
        }

        public async Task<WardVM> GetWardByIdAsync(int id)
        {
            return await _repo.GetWardByIdAsync(id);
        }

        public async Task SaveAsync(WardVM model)
        {
            await _repo.SaveAsync(model);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _repo.GetWardByIdAsync(id);

            if (data == null)
                return false;

            await _repo.DeleteAsync(id);
            return true;
        }
        public async Task<List<Ward>> GetWardNamesAsync()
        {
            return await _repo.GetWardNamesAsync();
        }
    }
}