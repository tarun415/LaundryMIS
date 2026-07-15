using LaudaryMis.Models;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaudaryMis.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> GeneratePayment(
            int agreementId,
            int hospitalId,
            int monthNo,
            int yearNo,
            int createdBy);

        Task<List<PaymentMaster>> GetPayments(
            int? agreementId,
            int? hospitalId,
            int? monthNo,
            int? yearNo,
            string status);

        Task<PaymentMaster?> GetPaymentById(int paymentId);

        Task<List<PaymentCalculation>> GetCalculations(int paymentId);

        Task<List<PaymentApprovalLog>> GetApprovalHistory(int paymentId);

        Task<List<PaymentDocument>> GetDocuments(int paymentId);

        Task<bool> ApprovePayment(
            int paymentId,
            int approvedBy,
            string remarks);

        Task<bool> RejectPayment(
            int paymentId,
            int rejectedBy,
            string remarks);

        Task UploadDocument(PaymentDocument document);
        Task<AgreementDetailsVM?> GetAgreementDetails(int agreementId);
        Task<List<SelectListItem>> GetAgreementDropdown();
        Task<GeneratePaymentVM> GetGeneratePaymentData(int hospitalId);
        Task<List<SelectListItem>> GetAgreementsByProvider(
    int hospitalId,
    int providerId);
        Task<PaymentCalculationVM> GetPaymentCalculation(
    int agreementId,
    int hospitalId,
    int monthNo,
    int yearNo,
    int bedOccupancy);
    }
}