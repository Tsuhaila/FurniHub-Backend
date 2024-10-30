using AutoMapper;
using FurniHub.Models.OrderModels;
using FurniHub.Models.OrderModels.DTOs;
using FurniHub.Models.PaymentModels;
using FurniHub.Models.UserModels;
using FurniHub.Services.JwtServices;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace FurniHub.Services.OrderServices
{
    public class OrderService:IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _hostUrl;
        public OrderService(IConfiguration configuration, IJwtService jwtService, ApplicationDbContext context,IMapper mapper)
        {
            _configuration = configuration;
            _jwtService = jwtService;
            _context = context;
            _mapper = mapper;
            _hostUrl = _configuration["HostUrl:Url"];

        }
        public async Task<string> RazorOrderCreate(long price)
        {
            try
            {
                Dictionary<string, object> input = new Dictionary<string, object>();
                Random random = new Random();
                string transctionId = random.Next(0, 1000).ToString();
                input.Add("amount", Convert.ToDecimal(price) * 100);
                input.Add("currency", "INR");
                input.Add("receipt", transctionId);

                string key = _configuration["RazorPay:KeyId"];
                string secret = _configuration["RazorPay:KeySecret"];

                RazorpayClient client = new RazorpayClient(key, secret);

                Razorpay.Api.Order order = client.Order.Create(input);
                var orderId = order["id"].ToString();
                return orderId;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RazorPayment(RazorPayDTO razorPayDTO)
        {
            if (razorPayDTO == null ||
                razorPayDTO.RazorPayId == null ||
                razorPayDTO.RazorPayOrdId == null ||
                razorPayDTO.RazorPaySig == null)
            {
                return false;
            }
            RazorpayClient client = new RazorpayClient(_configuration["RazorPay:KeyId"], _configuration["RazorPay:KeySecret"]);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("razorpay_payment_id", razorPayDTO.RazorPayId);
            parameters.Add("razorpay_order_id", razorPayDTO.RazorPayOrdId);
            parameters.Add("razorpay_signature", razorPayDTO.RazorPaySig);

            Utils.verifyPaymentSignature(parameters);
            return true;

        }
        public async Task<bool> CreateOrder(string token, OrderRequestDTO orderRequestDTO)
        {
            try
            {
                int userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("user is not valid");
                }
                if (orderRequestDTO.TransactionId == null && orderRequestDTO.OrderString == null)
                {
                    return false;
                }
                var cart = await _context.Cart
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                var order = new Models.OrderModels.Order
                {
                    userId = userId,
                    OrderDate = DateTime.Now,
                    CustomerName = orderRequestDTO.CustomerName,
                    CustomerEmail = orderRequestDTO.CustomerEmail,
                    CustomerCity = orderRequestDTO.CustomerCity,
                    CustomerPhone = orderRequestDTO.CustomerPhone,
                    HomeAddress = orderRequestDTO.HomeAddress,
                    OrderStatus = orderRequestDTO.OrderStatus,
                    OrderString = orderRequestDTO.OrderString,
                    TransactionId = orderRequestDTO.TransactionId,
                    OrderItems = cart?.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Quantity * ci.Product.Price,
                        
                    }).ToList()


                };

                _context.Order.Add(order);
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
                return true;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        public async Task<bool> UpdateOrderStatus(int orderId, AdminOrderResponseDTO orderDetails)
        {
            try
            {
                var order = await _context.Order.FindAsync(orderId);
                if (order != null)
                {
                    order.OrderStatus = orderDetails.OrderStatus;
                    await _context.SaveChangesAsync();
                    return true;

                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<OrderResponseDTO>> GetOrderDetails(string token)
        {
            try
            {
                int userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("user is not valid");
                }
                var orders = await _context.Order
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.userId == userId).ToListAsync();

                var orderDetails = new List<OrderResponseDTO>();
                foreach (var order in orders)
                {
                    foreach (var item in order.OrderItems)
                    {
                        var orderdetail = new OrderResponseDTO
                        {
                            Id = item.Id,
                            TotalPrice = item.TotalPrice,
                            Quantity = item.Quantity,
                            Image = _hostUrl + item.Product.Image,
                            ProductName = item.Product.Name,
                            OrderDate = order.OrderDate,
                            OrderStatus = order.OrderStatus,
                            OrderId = order.OrderString

                        };
                        orderDetails.Add(orderdetail);
                    }

                }
                return orderDetails;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
        public async Task<List<AdminOrderResponseDTO>> GetOrderDetailsForAdmin()
        {
            try
            {
                var orders = _context.Order.Include(o => o.OrderItems).ToList();
                if (orders != null)
                {
                    var orderDetails = orders.Select(o => new AdminOrderResponseDTO
                    {
                        Id = o.Id,
                        CustomerName = o.CustomerName,
                        CustomerEmail = o.CustomerEmail,
                        CustomerCity = o.CustomerCity,
                        CustomerPhone = o.CustomerPhone,
                        OrderId = o.orderId,
                        OrderDate = o.OrderDate,
                        OrderStatus = o.OrderStatus,
                        Orderstring = o.OrderString,
                        TransactionId = o.TransactionId,
                        HomeAddress = o.HomeAddress
                    }).ToList();
                    return orderDetails;

                }
                return new List<AdminOrderResponseDTO>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> TotalProductsPurchased()
        {
            try
            {
                int totalProducts = await _context.OrderItems.SumAsync(oi => oi.Quantity);
                return totalProducts;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<decimal> GetTotalRevenue()
        {
            try
            {
                var order = await _context.Order.Include(o => o.OrderItems).ToListAsync();
                if (order != null)
                {
                    var orederedItems = order.SelectMany(o => o.OrderItems);
                    var totalIncome = orederedItems.Sum(oi => oi.TotalPrice);
                    return totalIncome;

                }
                return 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

    }
}
