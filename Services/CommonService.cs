using LaudaryMis.Models;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using static LaudaryMis.ViewModels.CommonVM;

namespace LaudaryMis.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _repo;

        public CommonService(ICommonRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DropdownVM>> GetWards()
        {
            return await _repo.GetWards();
        }

        public async Task<List<LinenType>> GetLinenTypes()
        {
            return await _repo.GetLinenTypes();
        }

        public async Task<ProvidersVM> GetProviderByIdAsync(int id)
        {
            return await _repo.GetProviderByIdAsync(id);
        }

        public async Task<List<DropdownVM>> GetHospitalsByProvider(int providerId)
        {
            return await _repo.GetHospitalsByProvider(providerId);
        }

        public async Task<List<DropdownVM>> GetProviderByHospital(int hospitalId)
        {
            return await _repo.GetProviderByHospital(hospitalId);
        }

        public async Task<GetAgreementByHospitalVM> GetAgreementByHospital(int hospitalId)
        {
            return await _repo.GetAgreementByHospital(hospitalId);
        }
        public async Task<List<DeliverySummaryVM>>
GetDeliverySummaryReport()
        {
            return await _repo
                .GetDeliverySummaryReport();
        }

        public async Task<List<DeliveryHistoryVM>>
        GetDeliveryHistory(int pickupId)
        {
            return await _repo
                .GetDeliveryHistory(pickupId);
        }
    }
}