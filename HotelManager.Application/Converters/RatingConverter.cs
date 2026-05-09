using HotelManager.Application.DTO.Ratings;
using HotelManager.Domain.Entity.Ratings;

namespace HotelManager.Application.Converters
{
    public class RatingConverter
    {
        public RatingResponse EntityToRatingResponse(Rating rating)
        {

            return new RatingResponse
            {
                UserName = rating.Booking.Account.Username,
                BookingId = rating.BookingId,
                NumOfRating = rating.NumOfRating,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt
            };
        }
    }
}
