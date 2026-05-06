using HotelManager.Application.Converters;
using HotelManager.Application.DTO;
using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;
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

        public async Task<PagedResponse<RoomUserListResponse>> GetListRooms(int pageNumber = 1, int pageSize = 6)
        {
            var lists = await _roomRepository.GetListRoomsAvailable();
            var totalRecords = lists.Count();
            var pagedData = lists.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .Select(room => _roomConverter.EntityToUserListDto(room));
            return new PagedResponse<RoomUserListResponse>(pagedData, pageNumber, pageSize, totalRecords);
        }
        public async Task<RoomDetailsResponse> GetDetailsRoom(int id)
        {
            var room = await _roomRepository.GetRoomById(id);
            return _roomConverter.EntityToDto(room);
        }

        public async Task<PagedResponse<RoomUserListResponse>> GetRoomsAdvanced(CategoryRoom? category, bool? isAscending, int pageNumber = 1, int pageSize = 6)
        {
            // Lấy danh sách phòng có sẵn dưới dạng IQueryable (nếu Repo của bạn trả về IEnumerable thì dùng .AsQueryable())
            var availableRooms = await _roomRepository.GetListRoomsAvailable();
            var query = availableRooms.AsQueryable();

            // 1. Nếu người dùng có chọn Category -> Thêm điều kiện Lọc
            if (category.HasValue)
            {
                query = query.Where(r => r.Category == category.Value);
            }

            // 2. Nếu người dùng có chọn Sắp xếp -> Thêm điều kiện Sắp xếp
            if (isAscending.HasValue)
            {
                query = isAscending.Value
                    ? query.OrderBy(r => r.PricePerNight)
                    : query.OrderByDescending(r => r.PricePerNight);
            }

            // 3. Đếm tổng số bản ghi (sau khi đã lọc)
            var totalRecords = query.Count();

            // 4. Phân trang và map sang DTO
            var pagedData = query.Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .Select(room => _roomConverter.EntityToUserListDto(room))
                                 .ToList();

            return new PagedResponse<RoomUserListResponse>(pagedData, pageNumber, pageSize, totalRecords);
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
                var checkTime = date.ToDateTime(new TimeOnly(12, 0, 1));
                bool isAvailable = !bookings.Any(b => checkTime >= b.StartTime && checkTime < b.EndTime);
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
