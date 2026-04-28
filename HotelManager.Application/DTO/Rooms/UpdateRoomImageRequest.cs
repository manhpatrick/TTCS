namespace HotelManager.Application.DTO.Rooms
{
    public class UpdateRoomImageRequest
    {
        public string ImageUrl { get; set; }
        public bool IsThumbnail { get; set; } = false;
    }
}
