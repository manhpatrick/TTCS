using HotelManager.Application.DTO.Ratings;

namespace HotelManager.Application.IService
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingResponse>> GetRoomRatings(int roomId);
        Task AddRating(int accountId, RatingRequest request);
    }
}
