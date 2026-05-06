using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using System.Reflection;

namespace LaudaryMis.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IHospitalRepository _repo;

        public HospitalService(IHospitalRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<HospitalVM>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task SaveAsync(HospitalVM model)
        {
            if (string.IsNullOrWhiteSpace(model.HospitalName))
                throw new Exception("Hospital name required");

            if (model.HospitalId == null || model.HospitalId == 0)
                await _repo.InsertAsync(model);
            else
                await _repo.UpdateAsync(model);
        }
        public async Task CreateHospitalWithLogin(HospitalVM model)
        {
          if (string.IsNullOrWhiteSpace(model.HospitalName))
                throw new Exception("Hospital name required");

            if (model.HospitalId == null ||model.HospitalId == 0)
                await _repo.CreateHospitalWithLogin(model);
            else
                await _repo.UpdateAsync(model);
        }

        public async Task<HospitalVM?> GetHospitalByIdAsync(int id)
        {
            return await _repo.GetHospitalByIdAsync(id);
        }

        public async Task<List<Hospital>> GetHospitalsByDistrict(int districtId)
        {
            return await _repo.GetHospitalsByDistrict(districtId);
        }

        public async Task<List<District>> GetDistricts()
        {
            return await _repo.GetDistricts();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _repo.GetHospitalByIdAsync(id);
            if (data == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
        public async Task<List<GetHospital>> GetHospitalNamesAsync()
        {
            return await _repo.GetHospitalNamesAsync();
        }
    }
}