namespace HotelManager.Application.DTO.Ratings
{
    public class RatingRequest
    {
        public int BookingId { get; set; }
        public int NumOfRating { get; set; }
        public string Review { get; set; } = string.Empty;
    }
}
