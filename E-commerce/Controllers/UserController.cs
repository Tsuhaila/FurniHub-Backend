using FurniHub.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniHub.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }
        [Authorize(Roles ="admin")]
        [HttpGet("All-Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(users);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
                   
        }

        [Authorize(Roles ="admin")]
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user =await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPatch("Block-Unblock/{id}")]
        public async Task<IActionResult>BlockOrUnblockUser(int id)
        {
            try
            {
                var res=await _userService.BlockOrUnblockUser(id);
                return Ok(res);

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      
    }
}
