namespace LaudaryMis.Models
{
    public class Agreement
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int HospitalId { get; set; }

        public int BedCount { get; set; }
        public decimal RatePerBed { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? AgreementFile { get; set; }

        public bool IsActive { get; set; }
    }
}
