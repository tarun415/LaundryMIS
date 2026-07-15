using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaudaryMis.ViewModels
{
    public class GeneratePaymentVM
    {
        public int AgreementId { get; set; }

        public int HospitalId { get; set; }

        public int ProviderId { get; set; }

        public string HospitalName { get; set; }

        public string ProviderName { get; set; }

        public int BedCount { get; set; }

        public decimal RatePerBed { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public int MonthNo { get; set; }

        public int YearNo { get; set; }

        // New Properties

        public int BedOccupancy { get; set; }

        public decimal MonthlyBill { get; set; }

        public decimal AverageScore { get; set; }

        public decimal PaymentPercentage { get; set; }

        public decimal PayableAmount { get; set; }
        public decimal GrossPayable { get; set; }

        public decimal GSTPercentage { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal TDSPercentage { get; set; }

        public decimal TDSAmount { get; set; }

        public decimal NetPayable { get; set; }
    }

}
