using FurniHub.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];               
                var cartItems=await _cartService.GetCartItems(jwtToken);
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
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token?.Split(' ');
                var jwtToken= splitToken?[1];

                var res= await _cartService.AddToCart(jwtToken,productId);
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
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _cartService.RemoveFromCart(jwtToken, productId);
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
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _cartService.IncreaseQuantity(jwtToken, productId);
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
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _cartService.DecreaseQuantity(jwtToken, productId);
                return Ok(res);

            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message) ;
            }
           

        }
    }
}
