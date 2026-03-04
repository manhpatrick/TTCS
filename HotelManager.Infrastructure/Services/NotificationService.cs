using HotelManager.Application.Converters;
using HotelManager.Application.CustomException;
using HotelManager.Application.DTO.Notifications;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationConverter _notificationConverter;

        public NotificationService(INotificationRepository notificationRepository, NotificationConverter notificationConverter)
        {
            _notificationRepository = notificationRepository;
            _notificationConverter = notificationConverter;
        }

        public async Task AddNotification(NotificationRequest request)
        {
            await _notificationRepository.Add(_notificationConverter.DtoToEntity(request));
        }

        public async Task<IEnumerable<NotificationResponse>> GetNotifications(int accountId)
        {
            var list = await _notificationRepository.GetNotificationsByAccountId(accountId);
            return list.Select(n => _notificationConverter.EntityToDto(n, accountId)) ;
        }

        public async Task<IEnumerable<NotificationAdminResponse>> GetNotificationsAdmin()
        {
            var list = await _notificationRepository.GetAll();
            return list.Select(n => _notificationConverter.EntityToDtoAdmin(n));

        }

        public async Task UpdateNotification(int id, NotificationUpdateRequest request)
        {
            var notification = await _notificationRepository.GetById(id);
            if(request.Content != null) notification.ChangeContent(request.Content);
        }
        public async Task DeleteNotification(int id)
        {
            var notification = await _notificationRepository.GetById(id);
            await _notificationRepository.Remove(notification);
        }
        public async Task MarkIsRead(int notificationId,int accountId)
        {
            var notification = await _notificationRepository
                                    .GetByNotificationId(notificationId);
            var notificationAccount = notification.UserNotifications.FirstOrDefault(un => un.AccountId == accountId);
            if (notificationAccount == null) throw new NotExistsException("Tin nhắn này không gửi người này");
            notificationAccount.MarkIsRead();
            await _notificationRepository.SaveAsync();
        }
    }
}
