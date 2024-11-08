namespace FurniHub.Models.WishlistModels.DTOs
{
    public class WishlistResponseDTO
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal Price { get; set; }
        public decimal OfferPrice { get; set; }
        public string? CategoryName { get; set; }
        public string? ProductImage { get; set; }
        public string ProductId { get; set; }
    }
}
