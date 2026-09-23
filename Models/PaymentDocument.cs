using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.Models
{
    public class PaymentDocument
    {
        [Key]
        public int Id { get; set; }

        public int PaymentId { get; set; }

        public string DocumentType { get; set; }

        public string FileName { get; set; }

        public string? OriginalFileName { get; set; }

        public string FilePath { get; set; }

        public string? ContentType { get; set; }

        public long? FileSize { get; set; }

        public DateTime UploadedOn { get; set; }

        public int UploadedBy { get; set; }
    }
}