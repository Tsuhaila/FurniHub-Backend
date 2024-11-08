using FurniHub.Models.ApiResponseModel;
using FurniHub.Models.CartModels.DTOs;
using FurniHub.Models.CartModels;
using FurniHub.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FurniHub.Models.UserModels;

namespace FurniHub.Controllers
{
    [Route("api/users")]

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(new APIResponse<IEnumerable<OutPutUser>>(HttpStatusCode.OK, true, "fetch users successfully", users));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }
                   
        }

        [Authorize(Roles ="admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user =await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new APIResponse<OutPutUser>(HttpStatusCode.NotFound, false, "User with ID {id} not found.",user));
                }
                return Ok(new APIResponse<OutPutUser>(HttpStatusCode.OK, true, "User with ID {id} fetched successfully", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError,false, ex.Message,null));
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPatch("{id}/block-unblock")]
        public async Task<IActionResult>BlockOrUnblockUser(int id)
        {
            try
            {
                var res=await _userService.BlockOrUnblockUser(id);
                if (res==true)
                {
                    return Ok(new APIResponse<bool>(HttpStatusCode.OK, true, "user is blocked", res));
                }
                else
                {
                    return Ok(new APIResponse<bool>(HttpStatusCode.OK, true, "user is unblocked", res));
                }

            }catch(Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }
        }
      
    }
}
