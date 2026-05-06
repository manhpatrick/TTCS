namespace HotelManager.Application.CustomException.Rooms
{
    public class RoomNotAvailableException : AppException
    {
        public RoomNotAvailableException(string message) : base(message) { }
    }
}
