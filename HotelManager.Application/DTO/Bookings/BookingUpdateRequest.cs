using HotelManager.Domain.Entity.Bookings.Enum;

namespace HotelManager.Application.DTO.Bookings
{
    public  class BookingUpdateRequest
    {
        public BookingStatus bookingStatus { get; set; }
    }
}
