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
    }
}