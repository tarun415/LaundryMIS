using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class HospitalService
    {
        private readonly HospitalRepository _repo;

        public HospitalService(HospitalRepository repo)
        {
            _repo = repo;
        }

        public async Task SaveAsync(HospitalVM model)
        {
            if (string.IsNullOrWhiteSpace(model.HospitalName))
                throw new Exception("Hospital name required");

            await _repo.InsertAsync(model);
        }

        public async Task<IEnumerable<HospitalVM>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
