using Microsoft.AspNetCore.SignalR;

namespace HotelManager.Presentation.Hubs
{
    public class NotificationHub : Hub
    {
        // Dictionary để theo dõi kết nối user
        private static Dictionary<int, HashSet<string>> UserConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("sub")?.Value ?? Context.ConnectionId;
            
            if (int.TryParse(userId, out var accountId))
            {
                if (!UserConnections.ContainsKey(accountId))
                {
                    UserConnections[accountId] = new HashSet<string>();
                }
                UserConnections[accountId].Add(Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst("sub")?.Value ?? Context.ConnectionId;
            
            if (int.TryParse(userId, out var accountId) && UserConnections.ContainsKey(accountId))
            {
                UserConnections[accountId].Remove(Context.ConnectionId);
                if (UserConnections[accountId].Count == 0)
                {
                    UserConnections.Remove(accountId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Gửi thông báo đến một user cụ thể
        /// </summary>
        public async Task NotifyUser(int accountId, string message, string title = "Thông báo mới")
        {
            if (UserConnections.ContainsKey(accountId))
            {
                foreach (var connectionId in UserConnections[accountId])
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification", new
                    {
                        title,
                        message,
                        timestamp = DateTime.UtcNow,
                        isNew = true
                    });
                }
            }
        }

        /// <summary>
        /// Gửi thông báo đến tất cả các user (thường dùng cho thông báo hệ thống)
        /// </summary>
        public async Task NotifyAll(string message, string title = "Thông báo hệ thống")
        {
            await Clients.All.SendAsync("ReceiveNotification", new
            {
                title,
                message,
                timestamp = DateTime.UtcNow,
                isNew = true
            });
        }

        /// <summary>
        /// Gửi thông báo đến các user cụ thể (multiple users)
        /// </summary>
        public async Task NotifyMultipleUsers(List<int> accountIds, string message, string title = "Thông báo mới")
        {
            foreach (var accountId in accountIds)
            {
                await NotifyUser(accountId, message, title);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái badge thông báo
        /// </summary>
        public async Task UpdateNotificationBadge(int accountId, int unreadCount)
        {
            if (UserConnections.ContainsKey(accountId))
            {
                foreach (var connectionId in UserConnections[accountId])
                {
                    await Clients.Client(connectionId).SendAsync("UpdateNotificationBadge", new
                    {
                        unreadCount,
                        timestamp = DateTime.UtcNow
                    });
                }
            }
        }

        /// <summary>
        /// Gửi thông báo real-time khi có booking mới
        /// </summary>
        public async Task NotifyNewBooking(string roomName, string guestName, DateTime checkInDate)
        {
            await Clients.All.SendAsync("NewBooking", new
            {
                roomName,
                guestName,
                checkInDate,
                message = $"Có booking mới: {guestName} - {roomName} ({checkInDate:dd/MM/yyyy})",
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Gửi thông báo khi thanh toán hoàn thành
        /// </summary>
        public async Task NotifyPaymentCompleted(int accountId, string amount, string bookingId)
        {
            await NotifyUser(accountId, $"Thanh toán thành công: {amount}đ. Mã booking: {bookingId}", "Thanh toán");
        }
    }
}
