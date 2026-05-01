using HotelManager.Domain.Entity.Accounts;
using HotelManager.Domain.Entity.Notifications;
using System.ComponentModel;

namespace HotelManager.Domain.Entity.UserNotifications
{
    public class UserNotification
    {
        protected UserNotification() { }
        public int Id { get; private set; }
        public int AccountId { get; private set; }
        public Account Account { get; private set; }
        public int NotificationId { get; private set; }
        public Notification Notification { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime? ReadAt { get; private set; }

        public UserNotification(int accountId)
        {
            AccountId = accountId;
            IsRead = false;
        }
        public void MarkAsRead()
        {
            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }
    }
}
