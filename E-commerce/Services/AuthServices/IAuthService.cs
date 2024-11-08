using FurniHub.Models.UserModels.DTOs;

namespace FurniHub.Services.AuthServices
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegisterDTO userDTO);
        Task<LoginResponseDTO> Login(UserLoginDTO userDTO);
    }
}
