using HotelManager.Application.CustomException;
using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Ratings;
using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Entity.Rooms.Enum;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<Room>> GetRoomsByCategory(CategoryRoom category)
        {
            return await _dbSet.Where(r => r.Category == category).Include(r => r.RoomImages).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByStatus(RoomStatus status)
        {
            return await _dbSet.Where(r => r.RoomStatus == status).Include(r => r.RoomImages).ToListAsync();
        }
        public async Task<Room> GetRoomById(int id)
        {
            var exists = await _dbSet.FirstOrDefaultAsync(r => r.Id == id);
            if (exists == null) throw new NotExistsException("Room not exist");
            return exists;
        }
        
    }
}
