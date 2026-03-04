using HotelManager.Application.DTO.User;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManager.Presentation.Controllers.Customer
{
    public class UserController : CustomerController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPut("me")]
        public async Task<IActionResult> UpdateInfo([FromBody] UserRequest request)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _userService.UpdateUserInfo(accountId, request);
            return Ok();
        }
        [HttpGet("me")]
        public async Task<ActionResult<UserInfoResponse>> GetUserInfo()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);          
            return Ok(await _userService.GetUserInfo(accountId));
        }
    }
}
