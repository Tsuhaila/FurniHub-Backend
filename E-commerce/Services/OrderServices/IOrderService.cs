using FurniHub.Models.OrderModels.DTOs;
using FurniHub.Models.PaymentModels;

namespace FurniHub.Services.OrderServices
{
    public interface IOrderService
    {
        Task<string> RazorOrderCreate(long price);
        bool RazorPayment(RazorPayDTO razorPayDTO);
        Task<bool> CreateOrder(int userId, OrderRequestDTO orderRequestDTO);
        Task<List<OrderResponseDTO>> GetOrderDetails(int userId);
        Task<List<AdminOrderResponseDTO>> GetOrderDetailsForAdmin();
        Task<List<AdminOrderResponseDTO>> GetOrdersByIdForAdmin(int userId);
        Task<int> TotalProductsPurchased();
        Task<Decimal> GetTotalRevenue();
    }
}
