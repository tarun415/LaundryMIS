using LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class PaymentDetailsVM
    {
        public PaymentMaster Payment { get; set; } = null!;

        public List<PaymentDocument> Documents { get; set; }
            = new();

        public List<PaymentApprovalLog> History { get; set; }
            = new();

        // Provider's labour licence — approval is blocked while it is missing or expired
        public LabourLicenceStatus LabourLicence { get; set; } = new(true, null);
    }
}
