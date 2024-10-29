using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.OrderModels.DTOs
{
    public class OrderRequestDTO
    {
        
        [Required]
        public string? CustomerName { get; set; }
        [Required]
        [EmailAddress]
        public string? CustomerEmail { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "The CustomerPhone field must be a 10-digit phone number.")]
        public string? CustomerPhone { get; set; }
        [Required]
        public string? CustomerCity { get; set; }
        [Required]
        public string? HomeAddress { get; set; }
        [Required]
        public string? OrderString { get; set; }
        [Required]
        public string? OrderStatus { get; set; }
        [Required]
        public string? TransactionId { get; set; }
      
    }
}
