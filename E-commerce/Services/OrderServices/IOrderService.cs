using FurniHub.Models.OrderModels.DTOs;
using FurniHub.Models.PaymentModels;

namespace FurniHub.Services.OrderServices
{
    public interface IOrderService
    {
        Task<string> RazorOrderCreate(long price);
        bool RazorPayment(RazorPayDTO razorPayDTO);
        Task<bool> CreateOrder(string token, OrderRequestDTO orderRequestDTO);
        Task<bool> UpdateOrderStatus(int orderId, AdminOrderResponseDTO orderDTO);
        Task<List<OrderResponseDTO>> GetOrders(int userId);
        Task<List<OrderResponseDTO>> GetOrderDetails(string token);
        Task<List<AdminOrderResponseDTO>> GetOrderDetailsForAdmin();
        Task<int> TotalProductsPurchased();
        Task<Decimal> GetTotalRevenue();
    }
}
