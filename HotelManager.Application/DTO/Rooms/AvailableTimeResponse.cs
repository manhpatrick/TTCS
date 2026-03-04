namespace HotelManager.Application.DTO.Rooms
{
    public class AvailableTimeResponse
    {
        public DateOnly Date { get; set; }
        public bool IsAvailable { get; set; }
    }
}
