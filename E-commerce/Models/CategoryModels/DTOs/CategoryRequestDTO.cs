using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.Categories.DTOs
{
    public class CategoryRequestDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
