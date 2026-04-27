using HotelManager.Application.DTO.Rooms;
using HotelManager.Domain.Entity.Ratings;

namespace HotelManager.Application.Converters
{
    public class RatingConverter
    {
        public RatingResponse EntityToRatingResponse(Rating rating)
        {
            if (rating == null) return null;

            return new RatingResponse
            {
                BookingId = rating.BookingId,
                NumOfRating = rating.NumOfRating,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt
            };
        }
    }
}
