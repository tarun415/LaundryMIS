using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.Models
{
    public class WarningLetterDocument
    {
        public int DocumentId { get; set; }

        public int WarningId { get; set; }

        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        public DateTime UploadedOn { get; set; }

        public int UploadedBy { get; set; }

        public bool IsActive { get; set; }
    }
}