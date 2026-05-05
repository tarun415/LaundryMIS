using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IMonthlyBillService
    {
        // Form load (auto WPR score)
        Task<MonthlyBillVM> LoadBillFormAsync(
            int hospitalId, int month, int year);

        // Recalculate amounts (AJAX ke liye bhi)
        void ComputeAmounts(MonthlyBillVM vm);

        // Hospital actions
        Task<(bool Success, string Message, int BillId)>
            SaveDraftAsync(MonthlyBillVM vm, int userId);

        Task<(bool Success, string Message)>
            SubmitBillAsync(int billId, int userId);

        // CMS actions
        Task<(bool Success, string Message)>
            CMSActionAsync(int billId, int cmsUserId,
                           bool approve, string? remarks);

        // Read
        Task<MonthlyBillVM?> GetBillDetailAsync(int billId);

        Task<IEnumerable<BillListItemVM>> GetAllBillsAsync(
            string? status = null, int? hospitalId = null);

        Task<IEnumerable<BillListItemVM>> GetHospitalBillsAsync(
            int hospitalId);

        Task<MonthlyBillVM> LoadProviderBillFormAsync(
    int providerId, int hospitalId, int month, int year);

        // Provider bill submit to hospital
        Task<(bool Success, string Message)>
            SubmitToHospitalAsync(int billId, int providerId);

        // Hospital verify karta hai
        Task<(bool Success, string Message)>
            HospitalActionAsync(int billId, int hospitalUserId,
                                bool approve, string? remarks);

        // Provider ke bills
        Task<IEnumerable<BillListItemVM>> GetProviderBillsAsync(int providerId);

        // Hospital ke pending bills (verify ke liye)
        Task<IEnumerable<BillListItemVM>> GetBillsForHospitalVerifyAsync(int hospitalId);
    }
}