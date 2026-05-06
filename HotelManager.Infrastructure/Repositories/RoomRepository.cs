using HotelManager.Application.CustomException;
using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Entity.Rooms.Enum;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }
        
        public async Task<IEnumerable<Room>> GetListRooms()
        {
            return await _dbSet.Include(r => r.RoomImages).OrderBy(r => r.Name).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByCategory(CategoryRoom category)
        {
            return await _dbSet.Where(r => r.Category == category && r.RoomStatus == RoomStatus.Available).Include(r => r.RoomImages).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByStatus(RoomStatus status)
        {
            return await _dbSet.Where(r => r.RoomStatus == status).Include(r => r.RoomImages).ToListAsync();
        }
        public async Task<Room> GetRoomById(int id)
        {
            var exists = await _dbSet.Include(r => r.RoomImages).FirstOrDefaultAsync(r => r.Id == id);
            if (exists == null) throw new NotExistsException("Room not exist");
            return exists;
        }
        public async Task<IEnumerable<Room>> GetListRoomsAvailable()
        {
            return await _dbSet.Where(r => r.RoomStatus == RoomStatus.Available).Include(r => r.RoomImages).OrderBy(r => r.Name).ToListAsync();
        }
        public async Task<IEnumerable<Room>> GetAvailableRoomsSortedByPrice(bool isAscending = true)
        {
            var query = _dbSet.Where(r => r.RoomStatus == RoomStatus.Available)
                              .Include(r => r.RoomImages)
                              .AsQueryable(); 
            if (isAscending)
            {
                query = query.OrderBy(r => r.PricePerNight);
            }
            else
            {
                query = query.OrderByDescending(r => r.PricePerNight);
            }
            return await query.ToListAsync();
        }
    }
}
