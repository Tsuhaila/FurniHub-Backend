﻿using FurniHub.Models.ApiResponseModel;
using FurniHub.Models.UserModels.DTOs;
using FurniHub.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace FurniHub.Controllers
{
    [Route("api/auth")]
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
                if (!res)
                {
                    return Conflict("user already exist");
                }
                return Ok("registered successfully");
               
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
                var user = await _authService.Login(userDTO);

                var cookiesOption = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(3)
                };
                Response.Cookies.Append("AuthToken", user.Token, cookiesOption);

                var response = new APIResponse<LoginResponseDTO>(HttpStatusCode.OK, true, "logged in successfully", user);
                return Ok(response);

            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
