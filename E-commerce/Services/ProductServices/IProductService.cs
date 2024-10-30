using FurniHub.Models.ProductModels.DTOs;

namespace FurniHub.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllProducts();
        Task<ProductResponseDTO> GetProductById(int id);
        Task<List<ProductResponseDTO>> GetProductsByCategory(int categoryId);
        Task<List<ProductResponseDTO>> GetProductsByCategoryName(string categoryName);
        Task<string> CreateProduct(ProductRequestDTO productDTO,IFormFile image );
        Task<string> UpdateProduct(int id,ProductRequestDTO productDTO, IFormFile image );
        Task<string> DeleteProduct(int id);
    }
}
