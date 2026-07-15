namespace LaudaryMis.ViewModels
{
    public class AgreementDetailsVM
    {
        public int AgreementId { get; set; }

        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        public int HospitalId { get; set; }

        public string HospitalName { get; set; }

        public int BedCount { get; set; }

        public decimal RatePerBed { get; set; }

        public decimal ContractAmount { get; set; }

        public bool IsActive { get; set; }
    }
}
