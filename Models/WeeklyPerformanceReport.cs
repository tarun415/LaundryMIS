namespace LaudaryMis.Models
{
    public class WeeklyPerformanceReport
    {

        public int Id { get; set; }
        public int AgreementId { get; set; }
        public int ProviderId { get; set; }      // ← ADD THIS
        public int HospitalId { get; set; }      // ← ADD HospitalId

        public int Week { get; set; }
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public int TotalScore { get; set; }
        public int PaymentPercentage { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
