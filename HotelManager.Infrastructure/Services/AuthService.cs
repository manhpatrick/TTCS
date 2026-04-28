using HotelManager.Application.CustomException.Auth;
using HotelManager.Application.DTO.AppConfig;
using HotelManager.Application.DTO.Auth;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;
using HotelManager.Domain.Entity.Accounts;
using HotelManager.Domain.Entity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace HotelManager.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, IPasswordHasher<Account> passwordHasher,
            IOptions<JwtSettings> jwtsettings)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtsettings.Value;
        }
        public async Task<RegisterResponse> Register(RegisterRequest registerDTO){
            try{
                var exist = await _authRepository.GetAccountByUsername(registerDTO.Username);
                if (exist != null) 
                {
                    return new RegisterResponse { Success = false, Message = "Tài khoản đã tồn tại" };
                }

                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(registerDTO.Username, emailRegex))
                {
                    return new RegisterResponse { Success = false, Message = "Email không hợp lệ" };
                }

                var passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
                if (!Regex.IsMatch(registerDTO.Password, passwordRegex))
                {
                    return new RegisterResponse { Success = false, Message = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường và số" };
                }

                // 1. Tạo Account để đăng nhập
                var newAccount = new Account(registerDTO.Username);
                var passwordHash = _passwordHasher.HashPassword(newAccount, registerDTO.Password);
                newAccount.ChangePasswordHash(passwordHash);
                
                await _authRepository.Add(newAccount);
                await _authRepository.SaveAsync(); // Lưu Account xuống DB để lấy Id

                var newProfile = new User(newAccount.Id); // Tạo User với AccountId
                newProfile.ChangeEmail(registerDTO.Username); // Set email từ username
                newProfile.ChangeName("Tên người dùng"); // Set tên mặc định

                await _userRepository.Add(newProfile);
                await _userRepository.SaveAsync(); // Lưu User profile xuống DB

                return new RegisterResponse { Success = true, Message = "Đăng ký thành công! Bạn có thể đăng nhập ngay." };
            }
            catch (Exception ex)
            {
                return new RegisterResponse { Success = false, Message = $"Lỗi đăng ký: {ex.Message}" };
            }
        }
        public async Task<LoginResponse> Login(LoginRequest loginDTO)
        {
            var exist = await _authRepository.GetAccountByUsername(loginDTO.Username);
            if (exist == null) throw new UsernameNotExistException("Username not exist");
            var result = _passwordHasher.VerifyHashedPassword(exist, exist.PasswordHash, loginDTO.Password);
            if (result == PasswordVerificationResult.Failed) throw new PasswordIncorrectException("Password incorrect");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, exist.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, exist.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,exist.Username),
                new Claim(ClaimTypes.Role,exist.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_jwtSettings.ExpireMinutes)),
                signingCredentials: cred
            );
            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiredAt = token.ValidTo,
                FullName = exist.User?.Name
            };
        }

        
    }
}
