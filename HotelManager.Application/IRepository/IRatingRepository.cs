using HotelManager.Domain.Entity.Ratings;

namespace HotelManager.Application.IRepository
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<IEnumerable<Rating>> GetRatingsByRoomId(int roomId);
    }
}
