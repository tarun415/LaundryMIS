using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    public class GenerateWarningLetterVM
    {
        public int PaymentId { get; set; }

        public int AgreementId { get; set; }

        public string? AgreementNo { get; set; }

        public int HospitalId { get; set; }

        public string? HospitalName { get; set; }

        public int ProviderId { get; set; }

        public string? ProviderName { get; set; }

        public int? WPRId { get; set; }

        public int MonthNo { get; set; }

        public int YearNo { get; set; }

        public decimal PerformanceScore { get; set; }

        public decimal PaymentPercentage { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime WarningDate { get; set; } = DateTime.Today;

        [Required]
        public string? WarningLevel { get; set; }

        [Required]
        public string? Subject { get; set; }

        public string? Reason { get; set; }

        public string? Remarks { get; set; }

        public int CreatedBy { get; set; }
    }
}