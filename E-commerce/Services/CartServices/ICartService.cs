using FurniHub.Models.CartModels.DTOs;

namespace FurniHub.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartResponseDTO>> GetCartItems(string token);
        Task<bool> AddToCart(string token,int productId);
        Task<bool> RemoveFromCart(string token, int ProductId);

        Task<bool> IncreaseQuantity(string token, int ProductId);

        Task<bool> DecreaseQuantity(string token, int productId);
    }
}
