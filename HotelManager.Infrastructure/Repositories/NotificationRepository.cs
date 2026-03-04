using HotelManager.Application.CustomException;
using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Notifications;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<Notification>> GetNotificationsByAccountId(int accountId)
        {
            return await _dbSet.Where(n => n.UserNotifications.Any(un => un.AccountId == accountId)).Include(n => n.UserNotifications).ToListAsync();
        }
        public async Task<Notification> GetByNotificationId(int id)
        {
            var notification = await _dbSet.Include(n => n.UserNotifications)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null) throw new NotExistsException("Không tồn tại thông báo");
            return notification;
        }
    }
}
