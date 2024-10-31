using FurniHub.Models.WishlistModels.DTOs;

namespace FurniHub.Services.WishlistServices
{
    public interface IWishlistService
    {
        Task<string> ToggleWishlistItem(int userId, int productId);
        Task<List<WishlistResponseDTO>> GetWishlist(int userId);
    }
}
