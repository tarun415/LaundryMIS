namespace LaudaryMis.ViewModels
{
    public class WPRVM
    {
        public int AgreementId { get; set; }
        public int ProviderId { get; set; }
        public int HospitalId { get; set; }      // ← ADD HospitalId
        public int Week { get; set; }
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public int TotalScore { get; set; }

        public List<WPRDetailVM> Details { get; set; } = new();
    }

    public class WPRDetailVM
    {
        public int ParameterId { get; set; }
        public int Score { get; set; }
    }

    public class ParameterVM
    {
        public int Id { get; set; }
        public string ParameterName { get; set; } = "";
    }
   
}
