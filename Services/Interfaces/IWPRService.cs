using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IWPRService
    {
        Task<List<AgreementVM>> GetHospitalAgreements(int hospitalId);
        Task<(bool Success, string Message)> SubmitWPRAsync(WPRVM model);

        Task<bool> CheckWeeklyVerification(int hospitalId, int weekNo, int month, int year);
        Task<List<WeeklyPerformanceVM>>  GetWeeklyPerformanceData(int agreementId, int hospitalId,int weekNo, int month, int year);

        // CMS review
        Task<List<WprListItemVM>> GetListAsync(int? hospitalId, int? providerId, string? status, int? month, int? year);
        Task<WprReviewVM?> GetReviewAsync(int id);
        Task<CmsDashboardVM> GetCmsDashboardAsync(int hospitalId);
        Task<(bool Success, string Message)> CmsSaveAsync(int hospitalId, int userId, WprReviewPost post);
        Task<(bool Success, string Message)> CmsVerifyAsync(int hospitalId, int userId, int id);
        Task<(bool Success, string Message)> CmsVerifyMonthAsync(int hospitalId, int userId, int month, int year);
        Task<(bool Success, string Message, int Changed)> ApplyScoresAsync(
            WprReviewVM wpr, List<WprScoreVM> posted, int editorId, string editorRole, string? remarks, int? disputeId);
    }
}
