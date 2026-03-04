using Microsoft.AspNetCore.Http;

namespace HotelManager.Application.CustomException.Auth
{
    public class PasswordIsShortException : AppException
    {
        public PasswordIsShortException(string message) : base(message) { }
    }
}
