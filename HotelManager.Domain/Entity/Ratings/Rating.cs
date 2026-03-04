using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.Ratings
{
    public class Rating
    {
        protected Rating() { }

        public int Id { get; private set; }
        public int NumOfRating { get; private set; }
        public string Review { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int BookingId { get; private set; }
        public Booking Booking { get; private set; }

        internal Rating(Booking booking, int numOfRating, string review)
        {
            if (numOfRating < 1 || numOfRating > 5) throw new DomainException("Điểm đánh giá không hợp lệ");
            Booking = booking;
            BookingId = booking.Id;
            NumOfRating = numOfRating;
            Review = review;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
