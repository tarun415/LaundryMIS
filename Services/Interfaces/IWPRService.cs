using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IWPRService
    {
        Task<List<AgreementVM>> GetHospitalAgreements(int hospitalId);
        Task<(bool Success, string Message)> SubmitWPRAsync(WPRVM model);

        Task<bool> CheckWeeklyVerification(int weekNo, int month, int year);
        Task<List<WeeklyPerformanceVM>>  GetWeeklyPerformanceData(int agreementId, int hospitalId,int weekNo, int month, int year);
    }
}