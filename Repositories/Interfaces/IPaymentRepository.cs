using LaudaryMis.Models;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaudaryMis.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        

        //---------------------------------------------------
        // Payment Master
        //---------------------------------------------------

        Task<List<PaymentMaster>> GetPendingPayments();

        Task<PaymentMaster?> GetPaymentById(int paymentId);

        //---------------------------------------------------
        // Approval
        //---------------------------------------------------

        Task<bool> ApprovePayment(
            int paymentId,
            int approvedBy,
            string remarks);

        Task<bool> RejectPayment(
            int paymentId,
            int rejectedBy,
            string remarks);

        //---------------------------------------------------
        // Calculations
        //---------------------------------------------------

        Task<List<PaymentCalculation>> GetCalculations(int paymentId);

        //---------------------------------------------------
        // Approval History
        //---------------------------------------------------

        Task<List<PaymentApprovalLog>> GetApprovalHistory(int paymentId);

        //---------------------------------------------------
        // Documents
        //---------------------------------------------------

        Task UploadDocument(PaymentDocument document);

        Task<List<PaymentDocument>> GetDocuments(int paymentId);

        Task<List<PaymentMaster>> GetPayments(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string status);
        Task<AgreementDetailsVM?> GetAgreementDetails(int agreementId);
        Task<List<SelectListItem>> GetAgreementDropdown();
        Task<List<SelectListItem>> GetAgreementsByProvider( int hospitalId, int providerId);
      
        Task<GeneratePaymentVM> GetGeneratePaymentData(int hospitalId);

        Task<PaymentCalculationVM> GetPaymentCalculation(
            int agreementId,
            int hospitalId,
            int monthNo,
            int yearNo,
            int bedOccupancy);

        Task<bool> GeneratePayment(GeneratePaymentVM model);

    }
}