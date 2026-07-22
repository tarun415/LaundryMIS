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

        public int MonthNo { get; set; }

        public int YearNo { get; set; }

        //---------------------------------------------------
        // Agreement
        //---------------------------------------------------

        public int BedCount { get; set; }

        public decimal RatePerBed { get; set; }

        public decimal ContractAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        //---------------------------------------------------
        // User Input
        //---------------------------------------------------

        public int BedOccupancy { get; set; }

        //---------------------------------------------------
        // Calculation
        //---------------------------------------------------

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
