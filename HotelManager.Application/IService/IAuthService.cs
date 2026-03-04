using HotelManager.Application.DTO.Auth;

namespace HotelManager.Application.IService
{
    public interface IAuthService
    {
        Task Register(RegisterRequest registerDTO);
        Task<LoginResponse> Login(LoginRequest loginDTO);
    }
}
