namespace HotelManager.Application.DTO.Notifications
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public string ShortContent => Content.Length > 20 ? Content.Substring(0, 20) + "..." : Content;

    }
}
