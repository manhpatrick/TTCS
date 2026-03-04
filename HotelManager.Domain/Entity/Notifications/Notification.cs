using HotelManager.Domain.Entity.UserNotifications;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.Notifications
{
    public class Notification
    {
        protected Notification() { }
        public int Id { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private readonly List<UserNotification> _userNotifications = new();
        public IReadOnlyCollection<UserNotification> UserNotifications => _userNotifications.AsReadOnly();

        public Notification(string content, List<int> recipientIds)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new DomainException("Nội dung thông báo không được trống");
            if (recipientIds == null || !recipientIds.Any()) throw new DomainException("Phải có ít nhất 1 người nhận");
            Content = content;
            CreatedAt = DateTime.UtcNow;
            foreach (var userId in recipientIds)
            {
                var userNoti = new UserNotification(userId);
                _userNotifications.Add(userNoti);
            }
        }
        public void ChangeContent(string newContent)
        {
            if(string.IsNullOrWhiteSpace(newContent)) throw new DomainException("Nội dung thông báo không được trống");
            Content = newContent;
        }
    }
}
