namespace LaudaryMis.Models
{
    public class InvoiceListModel
    {
        public int InvoiceId { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string ProviderName { get; set; }

        public string HospitalName { get; set; }

        public decimal InvoiceAmount { get; set; }

        public string Status { get; set; }
    }
}
