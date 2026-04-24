using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IWPRRepository
    {
        Task<int> InsertAsync(WPRVM model, int hospitalId);
        Task<IEnumerable<WPRParameter>> GetParameters();
        Task<IEnumerable<AgreementVM>> GetHospitalAgreements(int hospitalId);
    }
}
