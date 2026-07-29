using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Repository
{
    public interface IWarningLetterRepository
    {
        Task<List<WarningLetterMaster>> GetWarningLetterList(
            int? agreementId,
            int? hospitalId,
            int? monthNo,
            int? yearNo,
            string? status);

        Task<GenerateWarningLetterVM> GetGenerateWarningLetterData(
            int paymentId);

        Task<GenerateWarningLetterResult> GenerateWarningLetter(
            GenerateWarningLetterVM model);

        Task<WarningLetterMaster> GetWarningLetterDetails(
            int warningId);

        Task<bool> UploadWarningLetterDocument(
            WarningLetterDocument model);

        Task<WarningLetterDocument> GetWarningLetterDocument(
            int warningId);
    }
}