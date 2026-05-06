using HotelManager.Application.Converters;
using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly RatingConverter _ratingConverter;
        public RatingService(IRatingRepository ratingRepository, RatingConverter ratingConverter)
        {
            _ratingRepository = ratingRepository;
            _ratingConverter = ratingConverter;
        }
        public async Task<IEnumerable<RatingResponse>> GetRoomRatings(int roomId)
        {
            var lists = await _ratingRepository.GetRatingsByRoomId(roomId);
            return lists.Select(rating => _ratingConverter.EntityToRatingResponse(rating));
        }
    }
}
