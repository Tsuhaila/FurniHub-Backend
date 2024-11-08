using AutoMapper;
using FurniHub.Models.OrderModels;
using FurniHub.Models.OrderModels.DTOs;
using FurniHub.Models.PaymentModels;
using FurniHub.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace FurniHub.Services.OrderServices
{
    public class OrderService:IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _hostUrl;
        public OrderService(IConfiguration configuration,ApplicationDbContext context,IMapper mapper)
        {
            _configuration = configuration;
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
                razorPayDTO.razorpay_payment_id == null ||
                razorPayDTO.razorpay_order_id == null ||
                razorPayDTO.razorpay_signature == null)
            {
                return false;
            }
            RazorpayClient client = new RazorpayClient(_configuration["RazorPay:KeyId"], _configuration["RazorPay:KeySecret"]);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("razorpay_payment_id", razorPayDTO.razorpay_payment_id);
            parameters.Add("razorpay_order_id", razorPayDTO.razorpay_order_id);
            parameters.Add("razorpay_signature", razorPayDTO.razorpay_signature);

            Utils.verifyPaymentSignature(parameters);
            return true;

        }
        public async Task<bool> CreateOrder(int userId, OrderRequestDTO orderRequestDTO)
        {
            try
            {
                if (orderRequestDTO.TransactionId == null && orderRequestDTO.OrderString == null)
                {
                    return false;
                }
                var cart = await _context.Cart
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                if (cart == null)
                {
                    return false;
                }
                var order = new Models.OrderModels.Order
                {
                    userId = userId,
                    OrderDate = DateTime.Now,
                    CustomerName = orderRequestDTO.CustomerName,
                    CustomerEmail = orderRequestDTO.CustomerEmail,
                    CustomerCity = orderRequestDTO.CustomerCity,
                    CustomerPhone = orderRequestDTO.CustomerPhone,
                    HomeAddress = orderRequestDTO.HomeAddress,
                    OrderString = orderRequestDTO.OrderString,
                    TransactionId = orderRequestDTO.TransactionId,
                    TotalAmount = orderRequestDTO.TotalAmount,
                    OrderItems = cart?.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Quantity * ci.Product.Price,
                        
                    }).ToList()


                };
                foreach(var cartItem in cart.CartItems)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);
                    if (product != null)
                        if (product.Quantity < cartItem.Quantity)
                        {
                           return false;
                        }
                    
                        product.Quantity -= cartItem.Quantity;
                    
                }

                _context.Orders.Add(order);
                _context.Cart.Remove(cart);
               
                await _context.SaveChangesAsync();
                return true;


            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException.Message);

            }
        }
        
        public async Task<List<OrderResponseDTO>> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await _context.Orders
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
                            TotalAmount=order.TotalAmount,
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
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi=>oi.Product)
                    .ToListAsync();
                if (orders != null)
                {
                    var orderDetails = orders.Select(o => new AdminOrderResponseDTO
                    {
                        Id = o.Id,
                        CustomerName = o.CustomerName,
                        CustomerEmail = o.CustomerEmail,
                        CustomerCity = o.CustomerCity,
                        CustomerPhone = o.CustomerPhone,
                        OrderDate = o.OrderDate,
                        Orderstring = o.OrderString,
                        TransactionId = o.TransactionId,
                        HomeAddress = o.HomeAddress,
                        TotalAmount = o.TotalAmount,
                        orderDetails=o.OrderItems.Select(oi=>new OrderResponseDTO
                        {
                            Image = oi.Product != null ? _hostUrl + oi.Product.Image : null,
                            ProductName = oi.Product?.Name ?? "Unknown Product",
                            TotalPrice =oi.TotalPrice
                            
                        }).ToList()
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
        public async Task<List<AdminOrderResponseDTO>> GetOrdersByIdForAdmin(int userId)
        {
            try
            {
                var orders = await _context.Orders
               .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Product)
               .Where(o => o.userId == userId).ToListAsync();
                if (orders != null)
                {
                    var orderDetails = orders.Select(o => new AdminOrderResponseDTO
                    {
                        Id = o.Id,
                        CustomerName = o.CustomerName,
                        CustomerEmail = o.CustomerEmail,
                        CustomerCity = o.CustomerCity,
                        CustomerPhone = o.CustomerPhone,
                        OrderDate = o.OrderDate,
                        Orderstring = o.OrderString,
                        TransactionId = o.TransactionId,
                        HomeAddress = o.HomeAddress,
                        TotalAmount = o.TotalAmount,
                        orderDetails = o.OrderItems.Select(oi => new OrderResponseDTO
                        {
                            Image = oi.Product != null ? _hostUrl + oi.Product.Image : null,
                            ProductName = oi.Product?.Name ?? "Unknown Product",
                            TotalPrice = oi.TotalPrice,
                            Quantity=oi.Quantity

                        }).ToList()

                    }).ToList();
                    return orderDetails;

                }
                return new List<AdminOrderResponseDTO>();

            }
            catch(Exception ex)
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
                var order = await _context.Orders.Include(o => o.OrderItems).ToListAsync();
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
