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

        public async Task Register(RegisterRequest registerDTO)
        {
            var exist = await _authRepository.GetAccountByUsername(registerDTO.Username);
            if (exist != null) throw new UsernameAlreadyExistException("Username already exist");
            if (registerDTO.Password.Length < 6) throw new PasswordIsShortException("Password must long or equal to 6 character");

            var newAccount = new Account(registerDTO.Username);
            var passwordHash = _passwordHasher.HashPassword(newAccount, registerDTO.Password);
            newAccount.ChangePasswordHash(passwordHash);
            await _authRepository.Add(newAccount);

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
                claims = claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_jwtSettings.ExpireMinutes)),
                signingCredentials: cred
            );
            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiredAt = token.ValidTo
            };
        }

        
    }
}
