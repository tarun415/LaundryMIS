using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IHospitalService
    {
        Task<IEnumerable<HospitalVM>> GetAllAsync();

        Task SaveAsync(HospitalVM model);


        Task<List<Hospital>> GetHospitalsByDistrict(int districtId);

        Task<List<District>> GetDistricts();

        Task<HospitalVM> GetHospitalByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}