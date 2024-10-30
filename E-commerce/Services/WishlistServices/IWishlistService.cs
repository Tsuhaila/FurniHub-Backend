using FurniHub.Models.WishlistModels.DTOs;

namespace FurniHub.Services.WishlistServices
{
    public interface IWishlistService
    {
        Task<string> AddOrRemoveWishlist(string token, int productId);
        Task<List<WishlistResponseDTO>> GetWishlist(string token);
    }
}
