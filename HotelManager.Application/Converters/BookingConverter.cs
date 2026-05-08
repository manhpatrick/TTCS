using HotelManager.Application.DTO.Bookings;
using HotelManager.Domain.Entity.Bookings;

namespace HotelManager.Application.Converters
{
    public class BookingConverter
    {
        public BookingTimeResponse EntityToBookingRoomResponse(Booking booking)
        {
            return new BookingTimeResponse
            {
                Id = booking.Id,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime
            };
        }
        public Booking DtoToEntity(int accountId,BookingRequest request, decimal RoomPriceAtBooking)
        {
            var booking = new Booking(accountId, request.RoomId, request.Note!, request.StartTime, request.EndTime, request.NumOfPeople,
                RoomPriceAtBooking);
            return booking;
        }
        public BookingDetailResponse EntityToDetailDto(Booking booking)
        {
            return new BookingDetailResponse
            {
                Id = booking.Id,
                Note = booking.Note,
                NumOfPeople = booking.NumOfPeople,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                CreatedAt = booking.CreatedAt,
                RoomPriceAtBooking = booking.RoomPriceAtBooking,
                BookingStatus = booking.BookingStatus,
                ApprovedAt = booking.ApprovedAt,
                RoomName = booking.Room?.Name,
                TotalPrice = booking.TotalPrice,
                review = booking.Rating?.Review,
                serviceLists = booking.ServiceUsages.Select(x => new ServiceListResponse
                {
                    Id = x.Id,
                    Name = x.Service.Name,
                    Quantity = x.Quantity,
                    Price = x.UnitPrice,
                }).ToList(),
            };
        }
    }
}
