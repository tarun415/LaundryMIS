using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IWPRService
    {
        Task<List<AgreementVM>> GetHospitalAgreements(int hospitalId);
        Task<(bool Success, string Message)> SubmitWPRAsync(WPRVM model);
    }
}