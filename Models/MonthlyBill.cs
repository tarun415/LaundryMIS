namespace LaudaryMis.Models
{
    public class MonthlyBill
    {
        public int Id { get; set; }
        public int AgreementId { get; set; }
        public int HospitalId { get; set; }
        public int ProviderId { get; set; }   // ← ADD KARO

        public byte BillingMonth { get; set; }
        public short BillingYear { get; set; }

        public int SanctionedBeds { get; set; }
        public decimal RatePerBedPerYear { get; set; }
        public decimal GSTPercent { get; set; } = 18m;

        public decimal WPRAvgScore { get; set; }
        public byte WPRWeeksConsidered { get; set; }
        public bool IsScoreOverridden { get; set; }
        public string? OverrideReason { get; set; }

        public decimal AnnualValueExGST { get; set; }
        public decimal AnnualValueInGST { get; set; }
        public decimal MonthlyGrossAmount { get; set; }
        public decimal PaymentBandPercent { get; set; }
        public decimal BasePayableAmount { get; set; }
        public decimal TDSPercent { get; set; } = 2m;
        public decimal TDSAmount { get; set; }
        public decimal AdditionalDeductions { get; set; }
        public string? DeductionRemarks { get; set; }
        public decimal NetPayableAmount { get; set; }

        // NAYA STATUS FLOW
        public string Status { get; set; } = "Draft";

        // Provider
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }

        // Hospital verify  ← YEH NAYA HAI
        public int? HospitalActionBy { get; set; }
        public DateTime? HospitalActionAt { get; set; }
        public string? HospitalRemarks { get; set; }

        // Admin/CMS
        public int? CMSActionBy { get; set; }
        public DateTime? CMSActionAt { get; set; }
        public string? CMSRemarks { get; set; }
    }
}