using HotelManager.Application.Converters;
using HotelManager.Application.CustomException.Rooms;
using HotelManager.Application.DTO.Bookings;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly BookingConverter _bookingConverter;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, BookingConverter bookingConverter, IServiceRepository serviceRepository,
            IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingConverter = bookingConverter;
            _serviceRepository = serviceRepository;
            _roomRepository = roomRepository;
        }
        public async Task AddBooking(int accountId, BookingRequest request)
        {
            var checkInTime = request.StartTime.Date.AddHours(12);
            var checkOutTime = request.EndTime.Date.AddHours(12);
            var available = await _bookingRepository.IsRoomAvailable(request.RoomId, checkInTime, checkOutTime);
            if (!available) throw new RoomNotAvailableException("Phòng đã được đặt trong khoảng thời gian này");
            var booking = _bookingConverter.DtoToEntity(accountId, request);
            foreach(AddServiceRequest service in request.ListService)
            {
                var sv = await _serviceRepository.GetById(service.Id);
                booking.AddService(sv.Id,service.Quantity, sv.Price);
            }
            var room = await _roomRepository.GetRoomById(request.RoomId);
            booking.ChangeRoom(room);
            await _bookingRepository.Add(booking);
            
            room.IncrementBookingCount();
            await _roomRepository.SaveAsync();
        }
        public async Task<IEnumerable<BookingTimeResponse>> GetBookingsByRoomId(int roomId, DateOnly from, DateOnly to)
        {
            var list = await _bookingRepository.GetBookingsInRange(roomId, from, to);
            return list.Select(b => _bookingConverter.EntityToBookingRoomResponse(b));
        }
        public async Task<IEnumerable<BookingDetailResponse>> GetBookingsByAccountId(int accountId)
        {
            var list = await _bookingRepository.GetBookingsByAccountId(accountId);
            return list.Select(b => _bookingConverter.EntityToDetailDto(b));
        }
    }
}
