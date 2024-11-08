using FurniHub.Models.CartModels;
using FurniHub.Models.OrderModels;
using FurniHub.Models.WishlistModels;
using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.UserModels.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderString { get; set; }     
        public string? TransactionId { get; set; }      
        public long TotalAmount { get; set; }

    }
}
