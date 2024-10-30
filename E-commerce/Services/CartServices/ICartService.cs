using FurniHub.Models.CartModels.DTOs;

namespace FurniHub.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartResponseDTO>> GetCartItems(string token);
        Task<string> AddToCart(string token,int productId);
        Task<string> RemoveFromCart(string token, int ProductId);
        Task<string> IncreaseQuantity(string token, int ProductId);
        Task<string> DecreaseQuantity(string token, int productId);
    }
}
