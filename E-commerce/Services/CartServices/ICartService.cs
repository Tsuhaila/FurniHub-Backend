using FurniHub.Models.CartModels.DTOs;

namespace FurniHub.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartResponseDTO>> GetCartItems(int userId);
        Task<string> AddToCart(int userId,int productId);
        Task<string> RemoveFromCart(int userId, int ProductId);
        Task<string> IncreaseQuantity(int userId, int ProductId);
        Task<string> DecreaseQuantity(int userId, int productId);
    }
}
