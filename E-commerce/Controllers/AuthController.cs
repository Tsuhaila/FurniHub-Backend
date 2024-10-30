using FurniHub.Models.UserModels.DTOs;
using FurniHub.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var res = await _authService.Register(userDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
          
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            try
            {
                var token=await _authService.Login(userDTO);
                var cookiesOption = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("AuthToken",token,cookiesOption);
                return Ok("loggined successfully");

            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = "Login failed", Error = ex.Message });


            }

        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Response.Cookies.Delete("AuthToken");
                return Ok("Logged out successfully");

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
