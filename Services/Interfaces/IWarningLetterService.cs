using LaudaryMis.Models;
using LaudaryMis.ViewModels;

namespace LaudaryMis.Services
{
    public interface IWarningLetterService
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

        Task<byte[]> GenerateWarningLetterPdf(
            int warningId,
            int? uploadedBy = null);

        Task<bool> UploadWarningLetterDocument(
            WarningLetterDocument model);

        Task<WarningLetterDocument> GetWarningLetterDocument(
            int warningId);
    }
}