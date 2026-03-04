namespace HotelManager.Application.CustomException
{
    public class NotExistsException : AppException
    {
        public NotExistsException(string message) : base(message) { }
    }
}
