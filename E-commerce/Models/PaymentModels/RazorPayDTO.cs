using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.PaymentModels
{
    public class RazorPayDTO
    {
        [Required]
        public string? RazorPayId { get; set; }
        [Required]
        public string? RazorPayOrdId { get;set; }
        [Required]
        public string? RazorPaySig { get;set;}
    }
}
