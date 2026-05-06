using HotelManager.Domain.Entity.Bookings.Enum;

namespace HotelManager.Application.DTO.Bookings
{
    public class BookingDetailResponse
    {
        public int Id { get; set; }
        public string Note { get; set; } = String.Empty;
        public int NumOfPeople { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public decimal RoomPriceAtBooking { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string RoomName { get; set; }
        public decimal TotalPrice { get; set; }
        public string review { get; set; } = String.Empty;
        public List<ServiceListResponse> serviceLists { get; set; } = new List<ServiceListResponse>();
    }
}
