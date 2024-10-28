using FurniHub.Models.UserModels.DTOs;
using FurniHub.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
            
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userDTO)
        {
           var s=await _authService.Register(userDTO);
            return Ok(s);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            try
            {
                var token=await _authService.Login(userDTO);
                return Ok(token);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
    }
}
