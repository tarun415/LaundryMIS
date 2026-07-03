namespace LaudaryMis.Models
{
    public class PaymentCalculation
    {
        public int Id { get; set; }

        public int PaymentId { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}