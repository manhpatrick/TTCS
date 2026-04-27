using HotelManager.Application.DTO.Auth;

namespace HotelManager.Application.IService
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequest registerDTO);
        Task<LoginResponse> Login(LoginRequest loginDTO);
    }
}
