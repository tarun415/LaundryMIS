namespace LaudaryMis.Models
{
    //public class WPREntry
    //{
    //    public int Id { get; set; }
    //    public int AgreementId { get; set; }
    //    public int HospitalId { get; set; }

    //    public DateTime WeekStart { get; set; }
    //    public DateTime WeekEnd { get; set; }

    //    public int TotalScore { get; set; }

    //    public string? Status { get; set; }   // Draft / Submitted
    //}
    public class WPREntry
    {
        public int Id { get; set; }

        public int AgreementId { get; set; }

        public int HospitalId { get; set; }

        public int? ProviderId { get; set; }

        public DateTime WeekStart { get; set; }

        public DateTime WeekEnd { get; set; }

        public int? TotalScore { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? MonthNo { get; set; }

        public int? YearNo { get; set; }

        public int? WeekNo { get; set; }

        public string? PerformanceGrade { get; set; }

        public string? Remarks { get; set; }

        public string? Status { get; set; }
    }
}
