using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IHospitalRepository
    {
        Task<IEnumerable<HospitalVM>> GetAllAsync();

        Task InsertAsync(HospitalVM model);
        Task UpdateAsync(HospitalVM model);

        Task<HospitalVM?> GetHospitalByIdAsync(int id);

        Task<List<Hospital>> GetHospitalsByDistrict(int districtId);

        Task<List<District>> GetDistricts();

        Task DeleteAsync(int id);
    }
}