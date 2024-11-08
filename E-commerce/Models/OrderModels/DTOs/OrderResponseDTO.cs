namespace FurniHub.Models.OrderModels.DTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Image {  get; set; }
        public string? OrderId { get; set; }
        public long TotalAmount { get; set; }
        
    }
}
