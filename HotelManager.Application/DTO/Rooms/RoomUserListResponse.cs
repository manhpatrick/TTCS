
namespace HotelManager.Application.DTO.Rooms
{
    public class RoomUserListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal AverageStar { get; set; }
        public int TotalReviews { get; set; }

    }
}
