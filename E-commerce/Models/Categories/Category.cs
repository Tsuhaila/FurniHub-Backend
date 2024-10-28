using FurniHub.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.Categories
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="category name is required")]
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }
        
    }
}
