using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.PaymentModels
{
    public class RazorPayDTO
    {
        [Required]
        public string? razorpay_payment_id { get; set; }
        [Required]
        public string? razorpay_order_id { get;set; }
        [Required]
        public string? razorpay_signature { get;set;}
    }
}
