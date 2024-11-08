using FurniHub.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.OrderModels
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int userId { get; set; }
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
        public virtual User? User { get; set; }
        public virtual List<OrderItem>? OrderItems { get; set; }


    }
}
