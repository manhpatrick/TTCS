using HotelManager.Application.DTO.Rooms;
using HotelManager.Domain.Entity.Rooms;

namespace HotelManager.Application.Converters
{
    public class RoomConverter
    {
        public Room DtoToEntity(RoomRequest request)
        {
            return new Room(request.Name, request.Description, request.Capacity, request.Category, request.RoomStatus, request.PricePerNight, request.imageUrls);
        }
        public RoomDetailsResponse EntityToDto(Room room)
        {
            return new RoomDetailsResponse
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                Capacity = room.Capacity,
                Category = room.Category,
                RoomStatus = room.RoomStatus,
                PricePerNight = room.PricePerNight,
                ListImageRoom = room.RoomImages.OrderBy(x => x.SortOrder)
                                                .Select(x => new RoomImageResponse
                                                {
                                                    Id = x.Id,
                                                    ImageUrl = x.ImageUrl,
                                                    IsThumbnail = x.IsThumbnail,
                                                    SortOrder = x.SortOrder
                                                })
                                                .ToList(),
                AverageStar = room.AverageStar,
                TotalReviews = room.TotalRatingCount

            };
        }
        public RoomUserListResponse EntityToUserListDto(Room room)
        {
            return new RoomUserListResponse
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                Capacity = room.Capacity,
                Category = room.Category,
                RoomStatus = room.RoomStatus,
                PricePerNight = room.PricePerNight,
                ThumbnailUrl = room.ThumbnailUrl,
                AverageStar = room.AverageStar,
                TotalRatingCount = room.TotalRatingCount,
                TotalReviews = room.TotalRatingCount
            };
        }
        public RoomAdminListResponse EntityToAdminListDto(Room room)
        {
            if (room == null) return null;

            return new RoomAdminListResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                PricePerNight = room.PricePerNight,
                ThumbnailUrl = room.ThumbnailUrl,
                AverageStar = room.AverageStar,
                TotalReviews = room.TotalRatingCount,

                // Các trường nội bộ dành riêng cho Admin
                Category = room.Category,
                RoomStatus = room.RoomStatus,
                BookingCount = room.BookingCount
            };
        }
    }
}
