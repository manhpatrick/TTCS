using HotelManager.Application.DTO;
using HotelManager.Application.DTO.Rooms;
using HotelManager.Domain.Entity.Rooms.Enum;

namespace HotelManager.Application.IService
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomAdminListResponse>> GetListRoomsAdmin();
        Task<PagedResponse<RoomUserListResponse>> GetListRooms(int pageNumber = 1, int pageSize = 6);
        Task<RoomDetailsResponse> GetDetailsRoom(int id);
        Task<PagedResponse<RoomUserListResponse>> GetRoomsAdvanced(CategoryRoom? category, bool? isAscending, int pageNumber = 1, int pageSize = 6);
        Task Add(RoomRequest request);
        Task Update(int id, RoomUpdateRequest request);
        Task Remove(int id);
        Task<List<AvailableTimeResponse>> CalculateAvailableTimes(int roomId, DateOnly from, DateOnly to);


    }
}
