using HotelManager.Application.DTO.Notifications;

namespace HotelManager.Application.IService
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationAdminResponse>> GetNotificationsAdmin();
        Task<IEnumerable<NotificationResponse>> GetNotifications(int accountId);
        Task AddNotification(NotificationRequest request);
        Task UpdateNotification(int id, NotificationUpdateRequest request);
        Task DeleteNotification(int id);
        Task MarkIsRead(int notificationId, int accountId);
    }
}
