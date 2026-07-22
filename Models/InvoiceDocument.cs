namespace LaundryMIS.Models
{
    namespace LaudaryMis.Models
    {
        public class InvoiceDocument
        {
            public int Id { get; set; }

            public int InvoiceId { get; set; }

            public string FileName { get; set; }

            public string FilePath { get; set; }

            public DateTime UploadedOn { get; set; }

            public int UploadedBy { get; set; }

            public bool IsActive { get; set; }
        }
    }
}