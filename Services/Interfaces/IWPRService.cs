using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IWPRService
    {
        Task<int> SaveAsync(WPRVM model, int hospitalId);
        Task<List<WPRParameter>> GetParameters();
        Task<List<AgreementVM>> GetHospitalAgreements(int hospitalId);
    }
}
