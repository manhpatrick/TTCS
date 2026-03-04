using HotelManager.Application.DTO.Rooms;
using HotelManager.Domain.Entity.Rooms.Enum;

namespace HotelManager.Application.IService
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomListResponse>> GetListRooms();
        Task<RoomDetailsResponse> GetDetailsRoom(int id);
        Task<IEnumerable<RoomListResponse>> GetRoomsByCategory(CategoryRoom category);
        Task<IEnumerable<RoomListResponse>> GetRoomsByStatus(RoomStatus status);
        Task Add(RoomRequest request);
        Task Update(int id, RoomUpdateRequest request);
        Task Remove(int id);
    }
}
