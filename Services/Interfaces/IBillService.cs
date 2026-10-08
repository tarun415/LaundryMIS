using LaudaryMis.ViewModels;

namespace LaudaryMis.Services.Interfaces
{
    public interface IBillService
    {
        // The vendor's hospitals + months, with whether the bill is ready, its version and print state
        Task<List<BillMonthVM>> GetMonthsAsync(int providerId);

        // The bill of one hospital + month, built from the verified WPRs. Null (with a reason) while the month is not fully verified.
        Task<(BillVM? Bill, string Message)> GetBillAsync(int providerId, int hospitalId, int month, int year);

        Task LogPrintAsync(BillVM bill, int userId);

        Task<List<BillPrintLogVM>> GetPrintLogAsync(int providerId);

        // Months of one hospital + year whose current bill the vendor has printed (and may dispute)
        Task<List<PrintedMonthVM>> GetDisputableMonthsAsync(int providerId, int hospitalId, int year);

        Task<List<OptionVM>> GetHospitalsAsync(int providerId);
    }
}
