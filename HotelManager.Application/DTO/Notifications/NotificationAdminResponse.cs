namespace HotelManager.Application.DTO.Notifications
{
    public class NotificationAdminResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ReceiverRequest> ListReceiver { get; set; } = new();
    }
}
