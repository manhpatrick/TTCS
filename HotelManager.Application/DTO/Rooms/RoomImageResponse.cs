namespace HotelManager.Application.DTO.Rooms
{
    public class RoomImageResponse
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsThumbnail { get; set; }
        public int SortOrder { get; set; }
    }
}
