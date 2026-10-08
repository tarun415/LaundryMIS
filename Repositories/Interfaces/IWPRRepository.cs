using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IWPRRepository
    {
        // AGREEMENTS
        Task<IEnumerable<AgreementVM>> GetHospitalAgreements(int hospitalId);

        // WPR
        Task<bool> WPRExistsAsync(int hospitalId, int week, string month, int year, string staffName);
        Task<int> InsertWPRAsync(WeeklyPerformanceReport wpr);
        Task InsertWPRDetailsAsync(IEnumerable<WPRDetail> details);

        Task<bool> CheckWeeklyVerification(int hospitalId, int weekNo, int month, int year);
        Task<List<WeeklyPerformanceVM>> GetWeeklyPerformanceData(
     int agreementId,  int hospitalId,  int weekNo,  int month,  int year);

        Task<int> InsertWPREntryAsync(WPREntry entry);

        // CMS review
        Task<List<WprListItemVM>> GetWprListAsync(int? hospitalId, int? providerId, string? status, int? month, int? year);
        Task<WprReviewVM?> GetWprReviewAsync(int id);
        Task<(int Pending, int Verified)> GetStatusCountsAsync(int hospitalId);
        Task<List<int>> GetPendingIdsAsync(int hospitalId, int month, int year);
        Task ApplyEditAsync(WprEditCommand cmd);
        Task<int> VerifyAsync(IEnumerable<int> ids, int hospitalId, int userId);

        Task<int> SaveWPRAsync(
    WeeklyPerformanceReport wpr,
    WPREntry entry,
    List<WPRDetail> details);
    }
}