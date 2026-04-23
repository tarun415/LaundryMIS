using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class DailyService : IDailyService
    {
        private readonly IDailyRepository _repo;

        public DailyService(IDailyRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> SaveAsync(DailyEntryVM model)
        {
            if (!model.Items.Any(x => x.DirtyCount > 0))
                throw new Exception("No data");

            return await _repo.InsertAsync(model);
        }

        public async Task<List<DailyEntryVM>> GetAllEntries()
        {
            return await _repo.GetAllEntries();
        }

        public async Task<List<Hospital>> GetHospitalsByProvider(int providerId)
        {
            return await _repo.GetHospitalsByProvider(providerId);
        }

        public async Task UpdateStatus(int id, string status)
        {
            await _repo.UpdateStatus(id, status);
        }

        public async Task<List<LinenType>> GetLinenTypes()
        {
            return await _repo.GetLinenTypes();
        }

        public async Task<IEnumerable<dynamic>> GetPendingEntries(int providerId)
        {
            return await _repo.GetPendingEntries(providerId);
        }

        public async Task<dynamic> GetEntryForDelivery(int id)
        {
            return await _repo.GetEntryWithItems(id);
        }

        public async Task<int> DeliverAsync(DeliveryVM model)
        {
            return await _repo.InsertDelivery(model);
        }
    }
}