using HotelManager.Application.DTO.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Application.IService
{
    public interface IBookingService
    {
        Task AddBooking(int accountId, BookingRequest request);
        Task<IEnumerable<BookingTimeResponse>> GetBookingsByRoomId(int roomId, DateOnly from, DateOnly to);
        Task<IEnumerable<BookingDetailResponse>> GetBookingsByAccountId(int accountId);
    }
}
