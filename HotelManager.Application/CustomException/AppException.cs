namespace HotelManager.Application.CustomException
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }
        protected AppException(string message) : base(message)
        {
        }
    }
}
