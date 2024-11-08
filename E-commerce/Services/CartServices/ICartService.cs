using FurniHub.Models.CartModels.DTOs;

namespace FurniHub.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartResponseDTO>> GetCartItems(int userId);
        Task<bool> AddToCart(int userId,int productId);
        Task<bool> RemoveFromCart(int userId, int ProductId);
        Task<bool> IncreaseQuantity(int userId, int ProductId);
        Task<bool> DecreaseQuantity(int userId, int productId);
    }
}
