namespace LaudaryMis.ViewModels
{
    public class HospitalVM
    {
        public int HospitalId { get; set; }
        public string? HospitalName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
