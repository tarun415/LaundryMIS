namespace LaudaryMis.ViewModels
{
    public class BillWorkflowLogVM
    {
        public string? FromStatus { get; set; }
        public string ToStatus { get; set; } = string.Empty;
        public string ActionByName { get; set; } = string.Empty;
        public DateTime ActionAt { get; set; }
        public string? Remarks { get; set; }
    }
}