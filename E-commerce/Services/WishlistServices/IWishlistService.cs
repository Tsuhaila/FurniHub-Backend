using FurniHub.Models.WishlistModels.DTOs;

namespace FurniHub.Services.WishlistServices
{
    public interface IWishlistService
    {
        Task<bool> ToggleWishlistItem(int userId, int productId);
        Task<List<WishlistResponseDTO>> GetWishlist(int userId);
    }
}
