namespace HotelManager.Application.CustomException.Auth
{
    public class PasswordIncorrectException : AppException
    {
        public PasswordIncorrectException(string message) : base(message) { }
    }
}
