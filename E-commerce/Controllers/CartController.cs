using FurniHub.Models.ApiResponseModel;
using FurniHub.Models.CartModels.DTOs;
using FurniHub.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace FurniHub.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            
        }

        [Authorize(Roles ="user")]
        [HttpGet("items")]
        public async Task<IActionResult>GetCartItems()
        {
            try
            {
                int userId = GetUserId();             
                var cartItems=await _cartService.GetCartItems(userId);
                if (cartItems.Count == 0)
                {
                    return Ok(new APIResponse<IEnumerable<CartResponseDTO>>(HttpStatusCode.BadRequest,false, "cart is empty", cartItems));

                }
                return Ok(new APIResponse<IEnumerable<CartResponseDTO>>(HttpStatusCode.OK, true, "fetched items successfully", cartItems));


            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));

            }
           
        }

        [Authorize(Roles ="user")]
        [HttpPost("items/{productId}")]
        public async Task<IActionResult>AddToCart(int productId)
        {
            try
            {
                int userId= GetUserId();
                var res= await _cartService.AddToCart(userId,productId);

                if (res == true)
                {
                    return Ok(new APIResponse<bool>(HttpStatusCode.OK, true, "item added to cart", res));
                }
               
                
                 return BadRequest(new APIResponse<bool>(HttpStatusCode.BadRequest, false, "item alteady exist", res));
                

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));

            }
         
        }

        [Authorize(Roles = "user")]
        [HttpDelete("items/{productId}")]
        public async Task<IActionResult>RemoveFromCart(int productId)
        {
            try
            {
                int userId= GetUserId();
                var res = await _cartService.RemoveFromCart(userId, productId);
                if (res == false)
                {
                    return BadRequest(new APIResponse<bool>(HttpStatusCode.OK, false, "item not found in cart", res));
                    
                }
                return Ok(new APIResponse<bool>(HttpStatusCode.OK, true, "item removed from cart", res));


            }
            catch(Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }

            
        }

        [Authorize(Roles = "user")]
        [HttpPut("items/{productId}/increase")]
        public async Task<IActionResult>IncreaseQuantity(int productId)
        {
            try
            {
                int userId= GetUserId();
                var res = await _cartService.IncreaseQuantity(userId, productId);
                if (res == false)
                {
                    return BadRequest(new APIResponse<string>(HttpStatusCode.BadRequest,false, "reached maximum quantity", "reached maximum quantity"));
                }
                return Ok(new APIResponse<string>(HttpStatusCode.OK, true,"Quantity increased","quantity increased"));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }
            
        }

        [Authorize(Roles = "user")]
        [HttpPut("items/{productId}/decrease")]
        public async Task<IActionResult>DecreaseQuantity(int productId)
        {
            try
            {
                int userId=GetUserId();
                var res = await _cartService.DecreaseQuantity(userId, productId);
                if (res == false)
                {
                    return BadRequest(new APIResponse<string>(HttpStatusCode.BadRequest, false, "reached minimum quantity", "reached minimum quantity"));
                }
                return Ok(new APIResponse<string>(HttpStatusCode.OK, true, "Quantity decreased", "quantity decreased"));


            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
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
