namespace LaudaryMis.ViewModels
{
    public class PaymentListVM
    {
        public int PaymentId { get; set; }

        public string PaymentNo { get; set; }

        public string HospitalName { get; set; }

        public int WeekNo { get; set; }

        public int PerformanceScore { get; set; }

        public string PerformanceGrade { get; set; }

        public decimal GrossPayable { get; set; }

        public decimal NetPayable { get; set; }

        public string Status { get; set; }
    }
}
