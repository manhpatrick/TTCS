using HotelManager.Domain.Entity.Rooms.Enum;
using System.ComponentModel.DataAnnotations;

public class RoomUpdateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public int? Capacity { get; set; }

    [EnumDataType(typeof(CategoryRoom))]
    public CategoryRoom? Category { get; set; }

    [EnumDataType(typeof(RoomStatus))]
    public RoomStatus? RoomStatus { get; set; }

    public decimal? PricePerNight { get; set; }
}
