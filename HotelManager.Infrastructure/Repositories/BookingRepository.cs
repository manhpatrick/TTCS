
using HotelManager.Application.DTO.Bookings;
using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Entity.Bookings.Enum;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HotelManager.Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<Booking>> GetBookingsByRoomId(int roomId)
        {
            var result = await _dbSet.Where(b => b.RoomId == roomId).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<Booking>> GetBookingsInRange(int roomId, DateOnly from, DateOnly to)
        {
            var fromDateTime = from.ToDateTime(TimeOnly.MinValue); // 00:00
            var toDateTime = to.ToDateTime(TimeOnly.MinValue);
            return await _dbSet
                    .Where(b =>
                            b.RoomId == roomId &&
                            b.BookingStatus != BookingStatus.Cancelled &&
                            fromDateTime < b.EndTime &&
                            toDateTime > b.StartTime)
                        .ToListAsync();
        }
        public async Task<bool> IsRoomAvailable(int roomId, DateTime start, DateTime end)
        {
            return !await _dbSet.AnyAsync(b => b.RoomId == roomId && b.BookingStatus != BookingStatus.Cancelled &&
                                            start < b.EndTime && end > b.StartTime);
        }
        public async Task<IEnumerable<Booking>> GetBookingsByAccountId(int accountId)
        {
            return await _dbSet.Where(b => b.AccountId == accountId)
                .Include(b => b.Room)
                .Include(b => b.Rating)
                .Include(b => b.ServiceUsages)
                .ThenInclude(su => su.Service)
                .OrderByDescending(b => b.CreatedAt).ToListAsync();
        }
    }
}
