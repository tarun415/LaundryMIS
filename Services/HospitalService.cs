using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly HospitalRepository _repo;

        public HospitalService(HospitalRepository repo)
        {
            _repo = repo;
        }

        //public async Task SaveAsync(HospitalVM model)
        //{
        //    if (string.IsNullOrWhiteSpace(model.HospitalName))
        //        throw new Exception("Hospital name required");

        //    await _repo.InsertAsync(model);
        //}

        public async Task<IEnumerable<HospitalVM>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }


        public async Task SaveAsync(HospitalVM model)
        {
            if (string.IsNullOrWhiteSpace(model.HospitalName))
                throw new Exception("Hospital name required");

            if (model.HospitalId == 0)
            {
                await _repo.InsertAsync(model);   // New
            }
            else
            {
                await _repo.UpdateAsync(model);   // Edit
            }
        }

        public async Task<HospitalVM> GetHospitalByIdAsync(int id)
        {
            return await _repo.GetHospitalByIdAsync(id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _repo.GetHospitalByIdAsync(id);

            if (data == null)
                return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
