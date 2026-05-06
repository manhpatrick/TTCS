namespace HotelManager.Application.CustomException.Auth
{
    public class UsernameNotExistException : AppException
    {
        public UsernameNotExistException(string message) : base(message) { }
    }
}
