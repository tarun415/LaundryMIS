using LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class PaymentVM
    {
        public PaymentMaster Payment { get; set; } = new();

        public List<PaymentCalculation> Calculations { get; set; } = new();

        public List<PaymentApprovalLog> ApprovalLogs { get; set; } = new();
    }
    public static class PaymentStatus
    {
        public const string Draft = "Draft";

        public const string Generated = "Generated";

        public const string Approved = "Approved";

        public const string InvoiceGenerated = "Invoice Generated";

        public const string Paid = "Paid";

        public const string Cancelled = "Cancelled";
    }
}