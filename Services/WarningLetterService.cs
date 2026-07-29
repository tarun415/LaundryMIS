using LaudaryMis.Documents;
using LaudaryMis.Models;
using LaudaryMis.Repositories;
using LaudaryMis.Repository;
using LaudaryMis.ViewModels;
using QuestPDF.Fluent;

namespace LaudaryMis.Services
{
    public class WarningLetterService : IWarningLetterService
    {
        private readonly IWarningLetterRepository _repository;

        public WarningLetterService(
            IWarningLetterRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<WarningLetterMaster>> GetWarningLetterList(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string? status)
        {
            return await _repository.GetWarningLetterList(
                agreementId,
                hospitalId,
                monthNo,
                yearNo,
                status);
        }
        public async Task<GenerateWarningLetterVM> GetGenerateWarningLetterData(
    int paymentId)
        {
            return await _repository.GetGenerateWarningLetterData(
                paymentId);
        }
        public async Task<GenerateWarningLetterResult> GenerateWarningLetter(
    GenerateWarningLetterVM model)
        {
            return await _repository.GenerateWarningLetter(model);
        }
        public async Task<WarningLetterMaster> GetWarningLetterDetails(
    int warningId)
        {
            return await _repository.GetWarningLetterDetails(
                warningId);
        }
        public async Task<bool> UploadWarningLetterDocument(
    WarningLetterDocument model)
        {
            return await _repository.UploadWarningLetterDocument(
                model);
        }
        public async Task<WarningLetterDocument> GetWarningLetterDocument(
    int warningId)
        {
            return await _repository.GetWarningLetterDocument(
                warningId);
        }
      
        public async Task<byte[]> GenerateWarningLetterPdf(
    int warningId)
        {
            // Get Warning Letter Details
            var warning = await _repository.GetWarningLetterDetails(warningId);

            if (warning == null)
                return null;

            // Generate PDF
            var document = new WarningLetterPdfDocument(warning);

            byte[] pdfBytes = document.GeneratePdf();

            // Create Folder
            string folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "WarningLetters");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Unique File Name
            string fileName =
                $"{warning.WarningNo}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            string fullPath = Path.Combine(folder, fileName);

            // Save PDF
            await File.WriteAllBytesAsync(fullPath, pdfBytes);

            // Save Document Entry
            WarningLetterDocument documentModel = new WarningLetterDocument
            {
                WarningId = warning.WarningId,
                FileName = fileName,
                FilePath = Path.Combine("WarningLetters", fileName).Replace("\\", "/"),
                UploadedBy = 1      // Later use Claims
            };

            await _repository.UploadWarningLetterDocument(documentModel);

            return pdfBytes;
        }
    }
}