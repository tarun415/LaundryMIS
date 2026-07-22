using LaudaryMis.Models;
using LaudaryMis.Repositories;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaudaryMis.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;

        public PaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }

        //---------------------------------------------------
        // Generate Payment
        //---------------------------------------------------

        public async Task<bool> GeneratePayment(
     GeneratePaymentVM model)
        {
            return await _repository.GeneratePayment(model);
        }

        //---------------------------------------------------
        // Pending Payments
        //---------------------------------------------------

        public async Task<List<PaymentMaster>> GetPendingPayments()
        {
            return await _repository.GetPendingPayments();
        }

        //---------------------------------------------------
        // Payment Details
        //---------------------------------------------------

        public async Task<PaymentMaster?> GetPaymentById(int paymentId)
        {
            return await _repository.GetPaymentById(paymentId);
        }

        //---------------------------------------------------
        // Approve
        //---------------------------------------------------

        public async Task<bool> ApprovePayment(
            int paymentId,
            int approvedBy,
            string remarks)
        {
            return await _repository.ApprovePayment(
                paymentId,
                approvedBy,
                remarks);
        }

        //---------------------------------------------------
        // Reject
        //---------------------------------------------------

        public async Task<bool> RejectPayment(
            int paymentId,
            int rejectedBy,
            string remarks)
        {
            return await _repository.RejectPayment(
                paymentId,
                rejectedBy,
                remarks);
        }

        //---------------------------------------------------
        // Calculation
        //---------------------------------------------------

        public async Task<List<PaymentCalculation>> GetCalculations(int paymentId)
        {
            return await _repository.GetCalculations(paymentId);
        }

        //---------------------------------------------------
        // Approval History
        //---------------------------------------------------

        public async Task<List<PaymentApprovalLog>> GetApprovalHistory(int paymentId)
        {
            return await _repository.GetApprovalHistory(paymentId);
        }

        //---------------------------------------------------
        // Upload Document
        //---------------------------------------------------

        public async Task UploadDocument(PaymentDocument document)
        {
            await _repository.UploadDocument(document);
        }

        //---------------------------------------------------
        // Documents
        //---------------------------------------------------

        public async Task<List<PaymentDocument>> GetDocuments(int paymentId)
        {
            return await _repository.GetDocuments(paymentId);
        }
        public async Task<List<PaymentMaster>> GetPayments(
    int? agreementId,
    int? hospitalId,
    int? monthNo,
    int? yearNo,
    string status)
        {
            return await _repository.GetPayments(
                agreementId,
                hospitalId,
                monthNo,
                yearNo,
                status);
        }
        public async Task<AgreementDetailsVM?> GetAgreementDetails(int agreementId)
        {
            return await _repository.GetAgreementDetails(agreementId);
        }
        public async Task<List<SelectListItem>> GetAgreementDropdown()
        {
            return await _repository.GetAgreementDropdown();
        }
        public async Task<GeneratePaymentVM> GetGeneratePaymentData(int hospitalId)
        {
            return await _repository.GetGeneratePaymentData(hospitalId);
        }
        public async Task<List<SelectListItem>> GetAgreementsByProvider(
    int hospitalId,
    int providerId)
        {
            return await _repository.GetAgreementsByProvider(
                hospitalId,
                providerId);
        }
        public async Task<PaymentCalculationVM> GetPaymentCalculation(
    int agreementId,
    int hospitalId,
    int monthNo,
    int yearNo,
    int bedOccupancy)
        {
            return await _repository.GetPaymentCalculation(
                agreementId,
                hospitalId,
                monthNo,
                yearNo,
                bedOccupancy);
        }
    }
}