using FurniHub.Models.ProductModels.DTOs;

namespace FurniHub.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllProducts();
        Task<ProductResponseDTO> GetProductById(int id);
        Task<List<ProductResponseDTO>> GetProductsByCategory(int categoryId);
        Task<List<ProductResponseDTO>> GetProductsByCategoryName(string categoryName);
        Task<bool> CreateProduct(ProductRequestDTO productDTO,IFormFile image );
        Task<bool> UpdateProduct(int id,ProductRequestDTO productDTO, IFormFile image );
        Task<bool> DeleteProduct(int id);
    }
}
