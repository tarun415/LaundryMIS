using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IWPRRepository
    {
        // AGREEMENTS
        Task<IEnumerable<AgreementVM>> GetHospitalAgreements(int hospitalId);

        // WPR
        Task<bool> WPRExistsAsync(int week, string month, int year, string staffName);
        Task<int> InsertWPRAsync(WeeklyPerformanceReport wpr);
        Task InsertWPRDetailsAsync(IEnumerable<WPRDetail> details);

        Task<bool> CheckWeeklyVerification(int weekNo, int month, int year);
        Task<List<WeeklyPerformanceVM>> GetWeeklyPerformanceData(
     int agreementId,  int hospitalId,  int weekNo,  int month,  int year);

        Task<int> InsertWPREntryAsync(WPREntry entry);

        Task<int> SaveWPRAsync(
    WeeklyPerformanceReport wpr,
    WPREntry entry,
    List<WPRDetail> details);
    }
}