using HotelManager.Application.DTO.Bookings;
using HotelManager.Domain.Entity.Bookings;

namespace HotelManager.Application.IService
{
    public interface IBookingService
    {
        Task<Booking> AddBooking(int accountId, BookingRequest request);
        Task<IEnumerable<BookingTimeResponse>> GetBookingsByRoomId(int roomId, DateOnly from, DateOnly to);
        Task<IEnumerable<BookingDetailResponse>> GetBookingsByAccountId(int accountId);
        Task<IEnumerable<BookingDetailAdminResponse>> GetListBookings();
        Task Update(int bookingId, BookingUpdateRequest request);
    }
}
