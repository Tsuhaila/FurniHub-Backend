using FurniHub.Services.WishlistServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            
        }
        [HttpPost("Add-Remove-items")]
        public async Task<IActionResult>AddOrRemoveItems(int userId,int productId)
        {
            try
            {
                var res = await _wishlistService.AddOrRemoveWishlist(userId, productId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("Get-wishlist")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            try
            {
                var res = await _wishlistService.GetWishlist(userId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
