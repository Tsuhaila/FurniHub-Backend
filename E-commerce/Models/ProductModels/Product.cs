using FurniHub.Models.CartModels;
using FurniHub.Models.Categories;
using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.ProductModels
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name is Required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product decription is Required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Image url is Required.")]
        [Url(ErrorMessage ="Invalid Url format")]
        public string Image { get; set; }
        [Range(0,double.MaxValue,ErrorMessage ="Price must be greater than or equal to 0.")]
        
        public decimal Price { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "offer price must be greater than or equal to 0.")]
        public decimal OfferPrice { get; set; }
        [Required(ErrorMessage ="category id is required.")]
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        [Range(0,5,ErrorMessage ="Rating must be between 0 and 5.")]
        public int Rating { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<CartItem> CartItems { get; set; }

        
    }
}
