namespace LaudaryMis.ViewModels
{
    public class WeeklyDeliveryReport
    {
        public DateTime ReportDate { get; set; }

        public int TotalPickups { get; set; }

        public int TotalCollectedQty { get; set; }

        public int TotalDeliveredQty { get; set; }

        public int TotalPendingQty { get; set; }

        public int FullyDeliveredCount { get; set; }

        public int PartialDeliveredCount { get; set; }
    }
    
}
