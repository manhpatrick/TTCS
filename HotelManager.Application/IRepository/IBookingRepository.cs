using HotelManager.Domain.Entity.Bookings;

namespace HotelManager.Application.IRepository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByRoomId(int roomId);
        Task<IEnumerable<Booking>> GetBookingsInRange(int roomId, DateOnly from, DateOnly to);

        Task<bool> IsRoomAvailable(int roomId, DateTime from, DateTime to);
        Task<IEnumerable<Booking>> GetBookingsByAccountId(int accountId);
    }
}
