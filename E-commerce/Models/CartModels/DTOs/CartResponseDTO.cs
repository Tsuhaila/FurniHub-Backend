using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FurniHub.Models.CartModels.DTOs
{
    public class CartResponseDTO
    {
        public int Id { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Price")]
        public decimal Price { get; set; }
        public decimal OfferPrice { get; set; }
        [DisplayName("Image URL")]
        public string Image { get; set; }
        [DisplayName("Total Price")]
        public string TotalPrice { get; set; }
        [DisplayName("Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

    }
}
