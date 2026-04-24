namespace LaudaryMis.ViewModels
{
    public class ProvidersVM
    {
        public int ProviderId { get; set; } = 0;
        public string? ProviderName { get; set; }
        public string? FirmName { get; set; }
        public int? RatePerBed { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
