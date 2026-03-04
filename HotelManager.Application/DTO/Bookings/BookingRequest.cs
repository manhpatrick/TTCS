
namespace HotelManager.Application.DTO.Bookings
{
    public class BookingRequest
    {
        public int RoomId { get; set; }
        public string? Note { get; set; }
        public int NumOfPeople { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal RoomPriceAtBooking { get; set; }
        public List<AddServiceRequest> ListService { get; set; } = new List<AddServiceRequest>();
    }
}
