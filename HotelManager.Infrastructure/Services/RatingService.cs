using HotelManager.Application.Converters;
using HotelManager.Application.CustomException;
using HotelManager.Application.DTO.Ratings;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly RatingConverter _ratingConverter;
        public RatingService(IRatingRepository ratingRepository, IBookingRepository bookingRepository, RatingConverter ratingConverter)
        {
            _ratingRepository = ratingRepository;
            _bookingRepository = bookingRepository;
            _ratingConverter = ratingConverter;
        }
        public async Task<IEnumerable<RatingResponse>> GetRoomRatings(int roomId)
        {
            var lists = await _ratingRepository.GetRatingsByRoomId(roomId);
            return lists.Select(rating => _ratingConverter.EntityToRatingResponse(rating));
        }
        public async Task AddRating(int accountId, RatingRequest request)
        {
            var booking = await _bookingRepository.GetById(request.BookingId);
            if (booking == null) throw new NotExistsException("Không tìm thấy đơn đặt phòng");
            if (booking.AccountId != accountId) throw new ForbiddenException("Bạn không có quyền đánh giá Booking này");
            booking.AddRating(request.NumOfRating, request.Review);
            await _bookingRepository.Update(booking.Id, booking);
        }
    }
}
