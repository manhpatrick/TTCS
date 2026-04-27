using HotelManager.Domain.Entity.Rooms.Enum;

namespace HotelManager.Application.DTO.Rooms
{
    public class RoomUserListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }
        public CategoryRoom Category { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public decimal PricePerNight { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal AverageStar { get; set; }
        public int TotalRatingCount { get; set; }
        public int TotalReviews { get; set; }

    }
}
