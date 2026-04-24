using LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class AgreementVM
    {
        public int Id { get; set; }
        public string? HospitalName { get; set; }
        public int ProviderId { get; set; }
        public int HospitalId { get; set; }

        public int BedCount { get; set; }
        public decimal RatePerBed { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IFormFile? AgreementFile { get; set; }

        public List<HospitalVM> Hospitals { get; set; } = new();
        public List<Provider> Providers { get; set; } = new();
    }
}
