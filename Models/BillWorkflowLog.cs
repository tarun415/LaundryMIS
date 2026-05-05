namespace LaudaryMis.Models
{
    public class BillWorkflowLog
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public string? FromStatus { get; set; }
        public string ToStatus { get; set; } = string.Empty;
        public int ActionBy { get; set; }
        public DateTime ActionAt { get; set; } = DateTime.Now;
        public string? Remarks { get; set; }
    }
}