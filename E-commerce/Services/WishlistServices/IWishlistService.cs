using FurniHub.Models.WishlistModels.DTOs;

namespace FurniHub.Services.WishlistServices
{
    public interface IWishlistService
    {
        Task<string> AddOrRemoveWishlist(int userId, int productId);
        Task<List<WishlistResponseDTO>> GetWishlist(int userId);
    }
}
