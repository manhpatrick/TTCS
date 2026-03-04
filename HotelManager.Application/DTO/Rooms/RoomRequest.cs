using HotelManager.Domain.Entity.Rooms.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManager.Application.DTO.Rooms;
public class RoomRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Capacity { get; set; }
    [Required]
    [Range(1,3)]
    [EnumDataType(typeof(CategoryRoom))]
    public CategoryRoom Category { get; set; }
    [Required]
    [Range(1, 3)]
    [EnumDataType(typeof(RoomStatus))]
    public RoomStatus RoomStatus { get; set; }
    [Required]
    public decimal PricePerNight { get; set; }
}
