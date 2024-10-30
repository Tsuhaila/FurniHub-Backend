using FurniHub.Services.WishlistServices;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost("Add-Remove-items")]
        public async Task<IActionResult>AddOrRemoveItems(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _wishlistService.AddOrRemoveWishlist(jwtToken, productId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("Get-wishlist")]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _wishlistService.GetWishlist(jwtToken);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
