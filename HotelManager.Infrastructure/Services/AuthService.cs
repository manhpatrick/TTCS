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
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;

namespace HotelManager.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        private readonly IMemoryCache _cache;
        private readonly MailSettings _mailSettings;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, IPasswordHasher<Account> passwordHasher,
            IOptions<JwtSettings> jwtsettings, IOptions<MailSettings> mailSettings, IMemoryCache cache)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtsettings.Value;
            _mailSettings = mailSettings.Value;
            _cache = cache;
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
                FullName = exist.User?.Name,
                Role = exist.Role.ToString()
            };
        }
        public async Task<bool> SendOtpAsync(string email)
        {
            // Kiểm tra user có tồn tại không (Giả sử Username đang lưu Email)
            var account = await _authRepository.GetAccountByUsername(email);
            if (account == null)
            {
                throw new Exception("Email không tồn tại trong hệ thống.");
            }

            // Tạo OTP ngẫu nhiên 6 số
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // Lưu OTP vào Cache, set thời gian sống là 5 phút
            _cache.Set($"OTP_{email}", otp, TimeSpan.FromMinutes(5));

            // Logic gửi Email bằng SMTP Gmail
            try
            {
                var smtpClient = new SmtpClient(_mailSettings.Host)
                {
                    Port = _mailSettings.Port,
                    Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
                    EnableSsl = true,
                    UseDefaultCredentials = false // Nên thêm dòng này để đảm bảo xác thực đúng
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName),
                    Subject = "Mã xác nhận khôi phục mật khẩu",
                    Body = $"Mã OTP khôi phục mật khẩu của bạn là: <b>{otp}</b>. Mã này sẽ hết hạn sau 5 phút.",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gửi email: {ex.Message}");
            }
        }

        // 2. HÀM KIỂM TRA OTP VÀ ĐỔI MẬT KHẨU
        public async Task<bool> ResetPasswordWithOtpAsync(VerifyOtpRequest request)
        {
            // Lấy OTP từ Cache ra để so sánh
            if (_cache.TryGetValue($"OTP_{request.Email}", out string cachedOtp))
            {
                if (cachedOtp == request.Otp)
                {
                    // OTP đúng -> Lấy tài khoản ra và đổi mật khẩu
                    var account = await _authRepository.GetAccountByUsername(request.Email);
                    if (account == null) throw new Exception("Tài khoản không tồn tại.");

                    // Hash mật khẩu mới (Nếu hệ thống của bạn có dùng hàm Hash, hãy thay bằng hàm Hash của bạn)
                    var newPasswordHash = _passwordHasher.HashPassword(account, request.NewPassword);

                    // Cập nhật pass (Tuỳ vào Entity Account của bạn có hàm đổi pass hay không)
                    account.ChangePasswordHash(newPasswordHash); // Cần đảm bảo có hàm này trong Entity Account

                    await _authRepository.Update(account.Id, account);

                    // Đổi xong thì xoá OTP khỏi Cache
                    _cache.Remove($"OTP_{request.Email}");

                    return true;
                }
            }

            throw new Exception("Mã OTP không chính xác hoặc đã hết hạn.");
        }

    }
}
