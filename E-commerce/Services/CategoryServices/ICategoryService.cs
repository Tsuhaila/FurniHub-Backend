using FurniHub.Models.Categories.DTOs;

namespace FurniHub.Services.CategoryServices
{
    public interface ICategoryService
    {
        public Task<bool>CreateCategory(CategoryRequestDTO categoryDTO);
        public Task<bool>UpdateCategory(int id,CategoryRequestDTO categoryDTO);
        public Task<List<CategoryResponseDTO>> GetAllCategories();
        public Task<bool> DeleteCategory(int id);
    }
}
