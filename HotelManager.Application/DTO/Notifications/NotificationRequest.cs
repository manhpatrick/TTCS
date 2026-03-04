namespace HotelManager.Application.DTO.Notifications
{
    public class NotificationRequest
    {
        public string Content { get; set; } = string.Empty;
        public List<ReceiverRequest> listReceiver { get; set; } = new();
    }
}
