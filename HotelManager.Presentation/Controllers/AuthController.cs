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

        [HttpPost("forgot-password/send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            try
            {
                var result = await _authService.SendOtpAsync(request.Email);
                return Ok(new { success = true, message = "OTP đã được gửi thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("forgot-password/verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                var result = await _authService.ResetPasswordWithOtpAsync(request);
                return Ok(new { success = true, message = "Đổi mật khẩu thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
