using FurniHub.Models.OrderModels.DTOs;
using FurniHub.Models.PaymentModels;
using FurniHub.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            
        }

        [Authorize]
        [HttpPost("Razor-Order-create")]
        public async Task<IActionResult> RazorOrderCreate(long price)
        {
            try
            {
                if(price<=0 || price > 100000)
                {
                    return BadRequest("Enter a valid amount!");
                }
                var orderId=await _orderService.RazorOrderCreate(price);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Razor-payment")]
        public IActionResult RazorPayment(RazorPayDTO razorPayDTO)
        {
            try
            {
                if (razorPayDTO == null)
                {
                    return BadRequest("razor pay details must not be full");
                }
                var res = _orderService.RazorPayment(razorPayDTO);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize]
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(OrderRequestDTO orderRequestDTO)
        {
            try
            {
                if (orderRequestDTO == null)
                {
                    return BadRequest();
                }
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                if (jwtToken == null || orderRequestDTO == null)
                {
                    return BadRequest();
                }
                var res =await  _orderService.CreateOrder(jwtToken, orderRequestDTO);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] AdminOrderResponseDTO orderDTO)
        {
            try
            {
                
                await _orderService.UpdateOrderStatus(orderId, orderDTO);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("Get-order-details")]
        public async Task<IActionResult> GetOrderDetails()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var jwtToken = token?.Split(' ')[1];
                var res = await _orderService.GetOrderDetails(jwtToken);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("get-order-details-for-Admin")]
        public async Task<IActionResult> GetOrderDetailsForAdmin()
        {
            try
            {
                var res=await _orderService.GetOrderDetailsForAdmin();
                return Ok(res);

            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);  
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("Total-products-purchased")]
        public async Task<IActionResult> TotalProductsPurchased()
        {
            try
            {
                var res = await _orderService.TotalProductsPurchased();
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("Get-Total-Revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var res=await _orderService.GetTotalRevenue();
                return Ok(res);

            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
