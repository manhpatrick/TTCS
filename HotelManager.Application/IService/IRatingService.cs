using HotelManager.Application.DTO.Rooms;

namespace HotelManager.Application.IService
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingResponse>> GetRoomRatings(int roomId);
    }
}
