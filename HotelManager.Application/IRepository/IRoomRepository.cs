using HotelManager.Domain.Entity.Ratings;
using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Entity.Rooms.Enum;

namespace HotelManager.Application.IRepository
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<IEnumerable<Room>> GetListRooms();
        Task<IEnumerable<Room>> GetRoomsByCategory(CategoryRoom category);
        Task<IEnumerable<Room>> GetRoomsByStatus(RoomStatus status);
        Task<Room> GetRoomById(int id);
        Task<IEnumerable<Room>> GetListRoomsAvailable();
        Task<IEnumerable<Room>> GetAvailableRoomsSortedByPrice(bool isAscending = true);
    }
}
