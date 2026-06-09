using LaudaryMis.Models;
using LaudaryMis.ViewModels;
using static LaudaryMis.ViewModels.CommonVM;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface ICommonRepository
    {
        Task<List<DropdownVM>> GetWards();

        Task<List<LinenType>> GetLinenTypes();

        Task<ProvidersVM> GetProviderByIdAsync(int id);

        Task<List<DropdownVM>> GetHospitalsByProvider(int providerId);

        Task<List<DropdownVM>> GetProviderByHospital(int hospitalId);
        Task<GetAgreementByHospitalVM> GetAgreementByHospital(int hospitalId);

        Task<List<DeliverySummaryVM>> GetDeliverySummaryReport();

        Task<List<DeliveryHistoryVM>>
            GetDeliveryHistory(int pickupId);
    }
}