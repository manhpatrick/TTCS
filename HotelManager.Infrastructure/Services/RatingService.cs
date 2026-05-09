using HotelManager.Application.Converters;
using HotelManager.Application.CustomException;
using HotelManager.Application.DTO.Notifications;
using HotelManager.Application.DTO.Ratings;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly RatingConverter _ratingConverter;
        private readonly INotificationService _notificationService;
        public RatingService(IRatingRepository ratingRepository,
            IRoomRepository roomRepository,
            IBookingRepository bookingRepository,
            INotificationService notificationService,
            RatingConverter ratingConverter)
        {
            _ratingRepository = ratingRepository;
            _bookingRepository = bookingRepository;
            _ratingConverter = ratingConverter;
            _notificationService = notificationService;
            _roomRepository = roomRepository;
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
            var room = await _roomRepository.GetById(booking.RoomId);
            if(room != null)
            {
                room.ChangeAverageStar(request.NumOfRating);
                await _roomRepository.Update(room.Id, room);
            }
            await _notificationService.AddNotification(new NotificationRequest
            {
                Title = $"Cảm ơn đánh giá của bạn cho đơn đặt phòng {booking.Room?.Name}",
                Content = $"Cảm ơn bạn đã sử dụng và đánh giá dịch vụ của chúng tôi, chúng tôi sẽ ghi nhận và tiếp thu" +
                        $"đánh giá của bạn",
                listReceiver = new List<ReceiverRequest>
                            {
                                new ReceiverRequest { Id = booking.AccountId }
                            }
            });
        }
    }
}
