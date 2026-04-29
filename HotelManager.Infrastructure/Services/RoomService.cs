using HotelManager.Application.Converters;
using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;
using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Entity.Rooms.Enum;

namespace HotelManager.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly RoomConverter _roomConverter;
        private readonly IBookingService _bookingService;


        public RoomService(IRoomRepository roomRepository, RoomConverter roomConverter, IBookingService bookingService)
        {
            _roomRepository = roomRepository;
            _roomConverter = roomConverter;
            _bookingService = bookingService;
        }

        public async Task Add(RoomRequest request)
        {
            var room = _roomConverter.DtoToEntity(request);
            await _roomRepository.Add(room);
        }

        public async Task<IEnumerable<RoomUserListResponse>> GetListRooms()
        {
            var lists = await _roomRepository.GetAllOrder(r => r.Name);
            return lists.Select(room => _roomConverter.EntityToUserListDto(room));
        }
        public async Task<RoomDetailsResponse> GetDetailsRoom(int id)
        {
            var room = await _roomRepository.GetRoomById(id);
            return _roomConverter.EntityToDto(room);
        }

        public async Task<IEnumerable<RoomUserListResponse>> GetRoomsByCategory(CategoryRoom category)
        {
            var lists = await _roomRepository.GetRoomsByCategory(category);
            return lists.Select(room => _roomConverter.EntityToUserListDto(room));
        }

        public async Task<IEnumerable<RoomUserListResponse>> GetRoomsByStatus(RoomStatus status)
        {
            var lists = await _roomRepository.GetRoomsByStatus(status);
            return lists.Select(room => _roomConverter.EntityToUserListDto(room));
        }

        public async Task Remove(int id)
        {
            var exists = await _roomRepository.GetById(id);
            await _roomRepository.Remove(exists);
        }
        

        public async Task Update(int id, RoomUpdateRequest request)
        {
            var exist = await _roomRepository.GetRoomById(id);
            if(request.Name != null) exist.ChangeRoomName(request.Name);
            if (request.Description != null) exist.ChangeDescription(request.Description);
            if (request.Capacity != null) exist.ChangeCapacity(request.Capacity.Value);
            if (request.Category != null) exist.ChangeCategoryRoom(request.Category.Value);
            if (request.RoomStatus != null) exist.ChangeRoomStatus(request.RoomStatus.Value);
            if (request.PricePerNight != null) exist.ChangePricePerNight(request.PricePerNight.Value);
            if (request.ImageUrls != null)
            {
                var incomingUrls = request.ImageUrls.Select(x => x.ImageUrl).ToList();

                var imagesToRemove = exist.RoomImages.Where(dbImg => !incomingUrls.Contains(dbImg.ImageUrl)).ToList();
                foreach(var imageUrl in imagesToRemove)
                {
                    exist.RemoveImage(imageUrl);
                }
                foreach(var imgRequest in request.ImageUrls)
                {
                    var existingImage = exist.RoomImages.FirstOrDefault(x => x.ImageUrl == imgRequest.ImageUrl);
                    if (existingImage != null)
                    {
                        // NẾU LÀ ẢNH CŨ -> GIỮ NGUYÊN
                        // Chỉ thay đổi trạng thái "Ảnh bìa" nếu người dùng chọn lại trên UI
                        exist.SetThumbnail(imgRequest.ImageUrl, imgRequest.IsThumbnail);
                    }
                    else
                    {
                        // NẾU LÀ ẢNH MỚI TINH -> GỌI HÀM THÊM
                        exist.AddImage(imgRequest.ImageUrl, imgRequest.IsThumbnail);
                    }
                }
            }
            await _roomRepository.SaveAsync();
        }
        

        public async Task<List<AvailableTimeResponse>> CalculateAvailableTimes(int roomId,DateOnly from,DateOnly to)
        {
            var bookings = await _bookingService.GetBookingsByRoomId(roomId, from, to);
            var result = new List<AvailableTimeResponse>();
            for(var date = from; date <= to; date = date.AddDays(1))
            {
                bool isAvailable = !bookings.Any(b => date.ToDateTime(TimeOnly.MinValue) >= b.StartTime &&
                                        date.ToDateTime(TimeOnly.MinValue) < b.EndTime);
                result.Add(new AvailableTimeResponse
                {
                    Date = date,
                    IsAvailable = isAvailable
                });
            }
            return result;
        }

        public async Task<IEnumerable<RoomAdminListResponse>> GetListRoomsAdmin()
        {
            var lists = await _roomRepository.GetAllOrder(r => r.Name);
            return lists.Select(room => _roomConverter.EntityToAdminListDto(room));
        }
    }
}
