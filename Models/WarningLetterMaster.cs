using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.Models
{
    public class WarningLetterMaster
    {
        public int WarningId { get; set; }

        public string? WarningNo { get; set; }

        public int AgreementId { get; set; }

        public int HospitalId { get; set; }

        public int ProviderId { get; set; }

        public int? WPRId { get; set; }

        public int? PaymentId { get; set; }

        [DataType(DataType.Date)]
        public DateTime WarningDate { get; set; }

        public string? WarningLevel { get; set; }

        public string? Subject { get; set; }

        public string? Reason { get; set; }

        public decimal? PerformanceScore { get; set; }
        public decimal? PaymentPercentage { get; set; }

        public string? Status { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        // Display Purpose

        public string? AgreementNo { get; set; }

        public string? HospitalName { get; set; }

        public string? ProviderName { get; set; }

        public string? MonthName { get; set; }

        public int MonthNo { get; set; }

        public int YearNo { get; set; }
    }
    public class GenerateWarningLetterResult
    {
        public int Result { get; set; }
        public int WarningId { get; set; }

        public string? WarningNo { get; set; }

        public string? ErrorMessage { get; set; }
    }
}

  
