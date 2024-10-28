using FurniHub.Services.CartServices;
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
        [HttpGet]
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
                return BadRequest(ex.Message);

            }
           
        }
        [HttpPost]
        public async Task<IActionResult>AddToCart(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token?.Split(' ');
                var jwtToken= splitToken?[1];

               var res= await _cartService.AddToCart(jwtToken,productId);
                return res ? Ok() : StatusCode(500, "An error occurred while incrementing quantity!");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
          

        }
        [HttpDelete]
        public async Task<IActionResult>RemoveFromCart(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _cartService.RemoveFromCart(jwtToken, productId);
                return res ? Ok() : StatusCode(500,"failed to remove item");

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }
        [HttpPut("Increment_Quantity")]
        public async Task<IActionResult>IncreaseQuantity(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _cartService.IncreaseQuantity(jwtToken, productId);
                return res ? Ok() : StatusCode(500);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPut("Decrement_Quantity")]
        public async Task<IActionResult>DecreaseQuantity(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _cartService.DecreaseQuantity(jwtToken, productId);
                return res ? Ok() : StatusCode(500);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message) ;
            }
           

        }
    }
}
