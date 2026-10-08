using LaudaryMis.ViewModels;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IBillRepository
    {
        // Every hospital + month the vendor has WPRs for, with verified / pending week counts
        Task<List<BillMonthVM>> GetMonthRowsAsync(int providerId, int? hospitalId = null, int? month = null, int? year = null);

        Task<BillAgreementInfo?> GetAgreementInfoAsync(int providerId, int hospitalId);

        Task<List<BillPrintLogVM>> GetPrintLogAsync(int providerId);
        Task InsertPrintLogAsync(int providerId, int hospitalId, int month, int year, int version,
                                 decimal avgScore, decimal netPayable, int userId);

        Task<List<DisputeCountVM>> GetDisputeCountsAsync(int providerId);
    }
}
