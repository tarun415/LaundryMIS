using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LaudaryMis.ViewModels
{
    public class VerifyDeliveryVM
    {
        public int DeliveryId { get; set; }

        public string LinenType { get; set; }

        public int CleanCount { get; set; }

        [Required]
        public int VerifiedQty { get; set; }

        public string? VerificationRemark { get; set; }

        [Required(ErrorMessage = "Log Book is mandatory")]
        public IFormFile LogBookFile { get; set; }
    }
}