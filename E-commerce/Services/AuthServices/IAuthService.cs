using FurniHub.Models.UserModels.DTOs;

namespace FurniHub.Services.AuthServices
{
    public interface IAuthService
    {
        Task<string> Register(UserRegisterDTO userDTO);
        Task<string> Login(UserLoginDTO userDTO);
    }
}
