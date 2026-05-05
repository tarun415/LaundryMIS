using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IMonthlyBillRepository
    {
        // WPR score auto-calculate
        Task<(decimal? AvgScore, int WeeksCount)> GetWPRAvgScoreAsync(
            int hospitalId, int month, int year);

        // Agreement info
        Task<MonthlyBillVM> GetAgreementInfoAsync(int hospitalId);

        // Bill CRUD
        Task<int> InsertBillAsync(MonthlyBill bill);
        Task UpdateBillAsync(MonthlyBill bill);
        Task<MonthlyBill?> GetBillByIdAsync(int billId);
        Task<MonthlyBill?> GetBillByHospitalMonthAsync(
                                int hospitalId, int month, int year);

        // Workflow
        Task UpdateBillStatusAsync(int billId, string newStatus,
                                   int actionBy, string? remarks,
                                   DateTime? cmsActionAt = null,
                                   int? cmsActionBy = null);
        Task InsertWorkflowLogAsync(BillWorkflowLog log);

        // Lists
        Task<IEnumerable<BillListItemVM>> GetAllBillsAsync(
            string? status = null, int? hospitalId = null);
        Task<IEnumerable<BillListItemVM>> GetHospitalBillsAsync(
            int hospitalId);

        // Workflow history
        Task<IEnumerable<BillWorkflowLogVM>> GetWorkflowLogAsync(int billId);

        Task<MonthlyBill?> GetBillByProviderHospitalMonthAsync(
    int providerId, int hospitalId, int month, int year);

        Task<IEnumerable<BillListItemVM>> GetProviderBillsAsync(int providerId);

        // Hospital verify ke liye
        Task<IEnumerable<BillListItemVM>> GetBillsForHospitalVerifyAsync(int hospitalId);

        // Status update (Hospital action ke liye)
        Task UpdateHospitalActionAsync(
            int billId, bool approve, int hospitalUserId, string? remarks);
        Task<MonthlyBillVM?> GetAgreementInfoByProviderHospitalAsync(
    int providerId, int hospitalId);
    }
}