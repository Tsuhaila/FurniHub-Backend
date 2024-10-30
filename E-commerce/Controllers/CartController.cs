using FurniHub.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            
        }

        [Authorize]
        [HttpGet("All-Items")]
        public async Task<IActionResult>GetCartItems()
        {
            try
            {
                int userId = GetUserId();             
                var cartItems=await _cartService.GetCartItems(userId);
                return Ok(cartItems);

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);

            }
           
        }

        [Authorize]
        [HttpPost("Add-Cart/{productId}")]
        public async Task<IActionResult>AddToCart(int productId)
        {
            try
            {
                int userId= GetUserId();
                var res= await _cartService.AddToCart(userId,productId);
                return Ok(res);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
         
        }

        [Authorize]
        [HttpDelete("Remove-cart/{productId}")]
        public async Task<IActionResult>RemoveFromCart(int productId)
        {
            try
            {
                int userId= GetUserId();
                var res = await _cartService.RemoveFromCart(userId, productId);
                return Ok(res);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            
        }

        [Authorize]
        [HttpPut("Increment_Quantity")]
        public async Task<IActionResult>IncreaseQuantity(int productId)
        {
            try
            {
                int userId= GetUserId();
                var res = await _cartService.IncreaseQuantity(userId, productId);
                return Ok(res);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }

        [Authorize]
        [HttpPut("Decrement_Quantity")]
        public async Task<IActionResult>DecreaseQuantity(int productId)
        {
            try
            {
                int userId=GetUserId();
                var res = await _cartService.DecreaseQuantity(userId, productId);
                return Ok(res);

            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message) ;
            }
        }
        private int GetUserId()
        {
            var strUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(strUserId, out var userId))
            {
                return userId;
            }
            throw new Exception("invalid user");
        }

    }
}
