namespace LaundryMIS.Models
{
    public class InvoiceDetailModel
    {
        public int InvoiceDetailId { get; set; }

        public int InvoiceId { get; set; }

        public int LinenTypeId { get; set; }

        public string LinenTypeName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }
    }
}