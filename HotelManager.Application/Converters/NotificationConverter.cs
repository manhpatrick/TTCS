using HotelManager.Application.DTO.Notifications;
using HotelManager.Domain.Entity.Notifications;
using HotelManager.Domain.Entity.UserNotifications;

namespace HotelManager.Application.Converters
{
    public class NotificationConverter
    {
        public Notification DtoToEntity(NotificationRequest request)
        {
            return new Notification(request.Title, request.Content, request.listReceiver.Select(b => b.Id).ToList());
        }
        public NotificationResponse EntityToDto(Notification notification, int accountId)
        {
            var userNotifications = notification.UserNotifications ?? new List<UserNotification>();
            var userNotification = userNotifications.FirstOrDefault(u => u.AccountId == accountId);
            return new NotificationResponse
            {
                Id = notification.Id,
                Title = notification.Title,
                Content = notification.Content,
                CreatedAt = notification.CreatedAt,
                IsRead = userNotification?.IsRead ?? false,
                ReadAt = userNotification?.ReadAt
            };
        }
        public NotificationAdminResponse EntityToDtoAdmin(Notification notification)
        {
            return new NotificationAdminResponse
            {
                Id = notification.Id,
                Title = notification.Title, 
                Content = notification.Content,
                CreatedAt = notification.CreatedAt,
                ListReceiver = notification.UserNotifications.Select(un => new ReceiverRequest
                                                            {
                                                                Id = un.AccountId,
                                                            }).ToList()
            };
        }
    }
}
