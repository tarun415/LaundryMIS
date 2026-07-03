using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.Models
{
    public class PaymentMaster
    {
        public int PaymentId { get; set; }

        public string PaymentNo { get; set; } = string.Empty;

        public int AgreementId { get; set; }

        public int ProviderId { get; set; }

        public int HospitalId { get; set; }

        public int WeekNo { get; set; }

        public int MonthNo { get; set; }

        public int YearNo { get; set; }

        public DateTime WeekStart { get; set; }

        public DateTime WeekEnd { get; set; }

        public decimal ContractAmount { get; set; }

        public decimal WeeklyContractAmount { get; set; }

        public int PerformanceScore { get; set; }

        public string? PerformanceGrade { get; set; }

        public decimal PaymentPercentage { get; set; }

        public decimal PenaltyAmount { get; set; }

        public decimal GrossPayable { get; set; }

        public decimal GSTPercentage { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal TDSPercentage { get; set; }

        public decimal TDSAmount { get; set; }

        public decimal NetPayable { get; set; }

        public string Status { get; set; } = "Draft";

        public string? Remarks { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public int? ApprovedBy { get; set; }
    }
}