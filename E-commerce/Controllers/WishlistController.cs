using FurniHub.Services.WishlistServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniHub.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            
        }
        [Authorize(Roles = "user")]
        [HttpPost("items")]
        public async Task<IActionResult>ToggleWishlistItem(int productId)
        {
            try
            {
                var userId = GetUserId();
                var res = await _wishlistService.ToggleWishlistItem(userId, productId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                int userId = GetUserId();
                var res = await _wishlistService.GetWishlist(userId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private int GetUserId()
        {
            var strUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(strUserId,out var userId))
            {
                return userId;
            }
            throw new Exception("invalid user");
        }


    }
}
