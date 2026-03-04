
using HotelManager.Application.DTO.Rooms;
using HotelManager.Domain.Entity.Rooms;

namespace HotelManager.Application.Converters
{
    public class RoomConverter
    {
        public Room DtoToEntity(RoomRequest request)
        {
            return new Room(request.Name, request.Description, request.Capacity, request.Category, request.RoomStatus, request.PricePerNight);
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
        public RoomListResponse EntityToListDto(Room room)
        {
            return new RoomListResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                PricePerNight = room.PricePerNight,
                AverageStar = room.AverageStar,
                TotalReviews = room.TotalRatingCount
            };
        }
    }
}
