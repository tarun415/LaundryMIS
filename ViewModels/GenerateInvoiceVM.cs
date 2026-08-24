
    namespace LaudaryMis.ViewModels
    {
        public class GenerateInvoiceVM
        {
            public int PaymentId { get; set; }

            public string PaymentNo { get; set; }

            public int AgreementId { get; set; }

            public int ProviderId { get; set; }

            public int HospitalId { get; set; }

            public string HospitalName { get; set; }

            public string ProviderName { get; set; }

            public int MonthNo { get; set; }

            public int YearNo { get; set; }

            public int SanctionedBeds { get; set; }

            public int BedOccupancy { get; set; }

            public decimal RatePerBed { get; set; }

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

            public DateTime InvoiceDate { get; set; } = DateTime.Today;

            public string? Remarks { get; set; }

            public int CreatedBy { get; set; }
        }
    }

