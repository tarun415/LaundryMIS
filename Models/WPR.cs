namespace LaudaryMis.Models
{
    public class WPREntry
    {
        public int Id { get; set; }
        public int AgreementId { get; set; }
        public int HospitalId { get; set; }

        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }

        public int TotalScore { get; set; }

        public string? Status { get; set; }   // Draft / Submitted
    }
}
