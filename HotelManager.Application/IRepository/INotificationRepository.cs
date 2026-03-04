using HotelManager.Domain.Entity.Notifications;

namespace HotelManager.Application.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByAccountId(int accountId);
        Task<Notification> GetByNotificationId(int id);
    }
}
