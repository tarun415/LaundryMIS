namespace LaudaryMis.Models
{
    public class WPRDetail
    {
        public int Id { get; set; }
        public int WPRId { get; set; }
        public int ParameterId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public int Score { get; set; }

    }
}
