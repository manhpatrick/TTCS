namespace HotelManager.Application.CustomException.Auth
{
    public class UsernameAlreadyExistException : AppException
    {
        public UsernameAlreadyExistException(string message) : base(message) { }
    }
}
