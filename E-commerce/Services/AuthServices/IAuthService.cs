using FurniHub.Models.UserModels.DTOs;

namespace FurniHub.Services.AuthServices
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegisterDTO userDTO);
        Task<string> Login(UserLoginDTO userDTO);
    }
}
