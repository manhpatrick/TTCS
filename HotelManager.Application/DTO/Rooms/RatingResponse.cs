
namespace HotelManager.Application.DTO.Rooms
{
    public class RatingResponse
    {
        public int BookingId { get; set; }
        public int NumOfRating { get; private set; }
        public string? Review { get; private set; }
        public DateTime CreatedAt { get; set; }
    }
}
