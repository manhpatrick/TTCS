using HotelManager.Application.DTO.Auth;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest dto)
        {
            var result = await _authService.Register(dto);
            
            if (result.Success)
            {
                return Ok(result); // Mã 200 kèm data thành công
            }
            
            return BadRequest(result); // Mã 400 kèm data báo lỗi (Sai pass, trùng email...)
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginRequest dto)
        {
            return Ok(await _authService.Login(dto));
        }
    }
}
