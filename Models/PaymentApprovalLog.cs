namespace LaudaryMis.Models
{
    public class PaymentApprovalLog
    {
        public int Id { get; set; }

        public int PaymentId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public int ActionBy { get; set; }

        public DateTime ActionDate { get; set; }
    }
}