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
        [Phone]
        public string? CustomerPhone { get; set; }
        [Required]
        public string? CustomerCity { get; set; }
        [Required]
        public string? HomeAddress { get; set; }
        [Required]
        public string? OrderString { get; set; }
        [Required]
        public string? TransactionId { get; set; }
        [Required]
        public long TotalAmount { get; set; }

    }
}
