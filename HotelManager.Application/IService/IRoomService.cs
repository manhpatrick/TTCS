using HotelManager.Application.DTO.Rooms;
using HotelManager.Domain.Entity.Rooms.Enum;

namespace HotelManager.Application.IService
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomAdminListResponse>> GetListRoomsAdmin();
        Task<IEnumerable<RoomUserListResponse>> GetListRooms();
        Task<RoomDetailsResponse> GetDetailsRoom(int id);
        Task<IEnumerable<RoomUserListResponse>> GetRoomsByCategory(CategoryRoom category);
        Task<IEnumerable<RoomUserListResponse>> GetRoomsByStatus(RoomStatus status);
        Task Add(RoomRequest request);
        Task Update(int id, RoomUpdateRequest request);
        Task Remove(int id);
        Task<List<AvailableTimeResponse>> CalculateAvailableTimes(int roomId, DateOnly from, DateOnly to);


    }
}
