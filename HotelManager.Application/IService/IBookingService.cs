using HotelManager.Application.DTO.Bookings;

namespace HotelManager.Application.IService
{
    public interface IBookingService
    {
        Task AddBooking(int accountId, BookingRequest request);
        Task<IEnumerable<BookingTimeResponse>> GetBookingsByRoomId(int roomId, DateOnly from, DateOnly to);
        Task<IEnumerable<BookingDetailResponse>> GetBookingsByAccountId(int accountId);
    }
}
