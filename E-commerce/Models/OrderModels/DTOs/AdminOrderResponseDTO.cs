using FurniHub.Models.CartModels.DTOs;

namespace FurniHub.Models.OrderModels.DTOs
{
    public class AdminOrderResponseDTO
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerPhone { get; set; }

        public string HomeAddress { get; set; }

        public string OrderId { get; set; }
        public string Orderstring { get; set; }

        public string OrderStatus { get; set; }

        public string TransactionId { get; set; }

        public DateTime OrderDate { get; set; }

        public List<CartResponseDTO> ProductsPurchased { get; set; }

    }
}
