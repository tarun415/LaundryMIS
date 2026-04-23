using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IHospitalRepository
    {

        Task InsertAsync(HospitalVM model);
        Task<IEnumerable<HospitalVM>> GetAllAsync();


    }
}
