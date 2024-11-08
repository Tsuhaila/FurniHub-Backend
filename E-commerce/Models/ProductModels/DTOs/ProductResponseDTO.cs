﻿namespace FurniHub.Models.ProductModels.DTOs
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal OfferPrice { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; } 
      
    }
}
