using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.Models
{
    public class PaymentMaster
    {
        public int PaymentId { get; set; }
        public string PaymentNo { get; set; }

        public int AgreementId { get; set; }
        public int ProviderId { get; set; }
        public int HospitalId { get; set; }

        public string HospitalName { get; set; }
        public string ProviderName { get; set; }

        public int MonthNo { get; set; }
        public int YearNo { get; set; }

        public int SanctionedBeds { get; set; }
        public int BedOccupancy { get; set; }

        public decimal RatePerBed { get; set; }
        public decimal MonthlyBill { get; set; }

        public decimal AverageScore { get; set; }
        public decimal PaymentPercentage { get; set; }

        public decimal GrossPayable { get; set; }

        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal TDSPercentage { get; set; }
        public decimal TDSAmount { get; set; }

        public decimal NetPayable { get; set; }

        public string Status { get; set; }
        public string Remarks { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string PaymentReferenceNo { get; set; }

        public bool InvoiceGenerated { get; set; }
        public int? InvoiceId { get; set; }

        public bool WarningGenerated { get; set; } = false;
        public int? WarningId { get; set; }
    }
}