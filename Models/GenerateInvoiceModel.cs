namespace LaudaryMis.Models
{
    public class GenerateInvoiceModel
    {
        public int ProviderId { get; set; }

        public int HospitalId { get; set; }

        public int AgreementId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
    public class GenerateInvoiceResult
    {
        public int Result { get; set; }

        public int InvoiceId { get; set; }

        public string InvoiceNo { get; set; }

        public string ErrorMessage { get; set; }
    }
}
