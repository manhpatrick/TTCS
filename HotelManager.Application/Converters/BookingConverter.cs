using HotelManager.Application.DTO.Bookings;
using HotelManager.Application.DTO.Ratings;
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
                ratingResponse = booking.Rating == null ? null : new RatingResponse
                {
                    BookingId = booking.Id,
                    NumOfRating = booking.Rating!.NumOfRating,
                    UserName = booking.Account!.Username,
                    CreatedAt = booking.Rating!.CreatedAt,
                    Review = booking.Rating!.Review
                },
                serviceLists = booking.ServiceUsages.Select(x => new ServiceListResponse
                {
                    Id = x.Id,
                    Name = x.Service.Name,
                    Quantity = x.Quantity,
                    Price = x.UnitPrice,
                }).ToList(),
            };
        }
        public BookingDetailAdminResponse EntityToDetailAdminDto(Booking booking)
        {
            return new BookingDetailAdminResponse
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
                CustomerName = booking.Account.User.Name,
                Email = booking.Account?.User?.Email,
                Phone = booking.Account?.User?.Phone,
                ratingResponse = booking.Rating == null ? null : new RatingResponse
                {
                    BookingId = booking.Id,
                    NumOfRating = booking.Rating!.NumOfRating,
                    UserName = booking.Account!.Username,
                    CreatedAt = booking.Rating!.CreatedAt,
                    Review = booking.Rating!.Review
                },
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
