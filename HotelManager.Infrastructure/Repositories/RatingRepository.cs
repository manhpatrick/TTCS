using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Ratings;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class RatingRepository :  GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Rating>> GetRatingsByRoomId(int roomId)
        {
            return await _dbSet.Include(r => r.Booking).ThenInclude(b => b.Account).Where(r => r.Booking.RoomId == roomId).ToListAsync();
        }
    }
}
