using Microsoft.AspNetCore.Http;
// Namespace này chứa class StatusCodes
namespace HotelManager.Application.CustomException.Auth
{
    public class UsernameAlreadyExistException : AppException
    {
        public UsernameAlreadyExistException(string message) : base(message) { }
    }
}
