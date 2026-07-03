namespace LaudaryMis.ViewModels
{
    public class WeeklyPerformanceVM
    {
        public int ParameterId { get; set; }

        public string ParameterName { get; set; }

        public int MaxScore { get; set; }

        public decimal? SystemScore { get; set; }

        public int? ManualScore { get; set; }

        public bool IsEditable { get; set; }

        public string Remarks { get; set; }
    }
}
