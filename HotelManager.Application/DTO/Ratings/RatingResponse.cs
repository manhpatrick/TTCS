namespace HotelManager.Application.DTO.Ratings
{
    public class RatingResponse
    {
        public int BookingId { get; set; }
        public int NumOfRating { get;  set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
