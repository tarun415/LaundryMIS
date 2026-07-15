using LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class PaymentDetailsVM
    {
        public PaymentMaster Payment { get; set; }

        public List<PaymentCalculation> Calculations { get; set; }
            = new();

        public List<PaymentDocument> Documents { get; set; }
            = new();

        public List<PaymentApprovalLog> History { get; set; }
            = new();
    }
}
