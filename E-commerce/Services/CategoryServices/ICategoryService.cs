using FurniHub.Models.Categories.DTOs;

namespace FurniHub.Services.CategoryServices
{
    public interface ICategoryService
    {
        public Task<List<CategoryResponseDTO>> GetAllCategories();
        public Task<string>CreateCategory(CategoryRequestDTO categoryDTO);
        public Task<string>UpdateCategory(int id,CategoryRequestDTO categoryDTO);
        public Task<string> DeleteCategory(int id);
    }
}
