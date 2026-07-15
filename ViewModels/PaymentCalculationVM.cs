namespace LaudaryMis.ViewModels
{
    public class PaymentCalculationVM
    {
        public decimal RatePerBed { get; set; }

        public int BedOccupancy { get; set; }

        public decimal MonthlyBill { get; set; }

        public decimal AverageScore { get; set; }

        public decimal PaymentPercentage { get; set; }

        public decimal GrossPayable { get; set; }

        public decimal GSTPercentage { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal TDSPercentage { get; set; }

        public decimal TDSAmount { get; set; }

        public decimal NetPayable { get; set; }
    }
}
