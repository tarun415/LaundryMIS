using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IHospitalService
    {
        Task SaveAsync(HospitalVM model);
        Task<IEnumerable<HospitalVM>> GetAllAsync();
    }
}
