using FurniHub.Models.OrderModels.DTOs;
using FurniHub.Models.PaymentModels;
using FurniHub.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniHub.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }

        [Authorize]
        [HttpPost("razor")]
        public async Task<IActionResult> RazorOrderCreate(long price)
        {
            try
            {
                if (price <= 0 || price > 100000)
                {
                    return BadRequest("Enter a valid amount!");
                }
                var orderId = await _orderService.RazorOrderCreate(price);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("payment")]
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
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderRequestDTO orderRequestDTO)
        {
            try
            {
                if (orderRequestDTO == null)
                {
                    return BadRequest();
                }
                int userId = GetUserId();
                if (userId == 0 || orderRequestDTO == null)
                {
                    return BadRequest();
                }
                var res = await _orderService.CreateOrder(userId, orderRequestDTO);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            try
            {
                int userId = GetUserId();
                var res = await _orderService.GetOrderDetails(userId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetOrderDetailsForAdmin()
        {
            try
            {
                var res = await _orderService.GetOrderDetailsForAdmin();
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet("admin/{userId}")]
        public async Task<IActionResult> GetOrdersByIdForAdmin(int userId)
        {
            try
            {
                var res =await _orderService.GetOrdersByIdForAdmin(userId);
                return Ok(res);

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin/total-products")]
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

        [Authorize(Roles = "admin")]
        [HttpGet("admin/revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var res = await _orderService.GetTotalRevenue();
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
            if (int.TryParse(strUserId, out var userId))
            {
                return userId;
            }
            throw new Exception("invalid user");
        }
    }
}
