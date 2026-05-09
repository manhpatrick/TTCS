using HotelManager.Application.DTO.Notifications;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HotelManager.Presentation.Hubs;

namespace HotelManager.Presentation.Controllers.Admin
{
    public class NotificationController : AdminController
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public NotificationController(INotificationService notificationService, IHubContext<NotificationHub> notificationHub)
        {
            _notificationService = notificationService;
            _notificationHub = notificationHub;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationAdminResponse>>> GetNotificationsAdmin()
        {
            return Ok(await _notificationService.GetNotificationsAdmin());
        }
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] NotificationRequest request)
        {
            // 1. Lưu thông báo vào Database (Service của bạn tự lo việc map All hay Cá nhân dựa vào listReceiver)
            await _notificationService.AddNotification(request);

            // 2. Logic kiểm tra để Push Real-time
            if (request.listReceiver == null || !request.listReceiver.Any())
            {
                // TRƯỜNG HỢP A: GỬI TẤT CẢ (Global)
                await _notificationHub.Clients.All.SendAsync("ReceiveNotification", new
                {
                    title = request.Title ?? "Thông báo mới",
                    message = request.Content,
                    timestamp = DateTime.UtcNow,
                    isNew = true
                });
            }
            else
            {
                // TRƯỜNG HỢP B: GỬI NHÓM NGƯỜI CHỈ ĐỊNH (Cá nhân)

                // Trích xuất danh sách ID từ mảng listReceiver thành kiểu chuỗi (SignalR yêu cầu chuỗi)
                IReadOnlyList<string> userIds = request.listReceiver.Select(r => r.Id.ToString()).ToList();

                // Sử dụng Clients.Users(...) để gửi một phát cho TẤT CẢ những người trong mảng
                await _notificationHub.Clients.Users(userIds).SendAsync("ReceiveUserNotification", new
                {
                    title = request.Title ?? "Thông báo cá nhân",
                    message = request.Content,
                    timestamp = DateTime.UtcNow,
                    isNew = true
                });
            }

            return Ok(new { message = "Thông báo đã được phát hành thành công" });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification([FromRoute]int id, [FromBody] NotificationUpdateRequest request)
        {
            await _notificationService.UpdateNotification(id, request);
            
            // Thông báo cập nhật
            await _notificationHub.Clients.All.SendAsync("NotificationUpdated", new
            {
                notificationId = id,
                updatedContent = request.Content,
                timestamp = DateTime.UtcNow
            });

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] int id)
        {
            await _notificationService.DeleteNotification(id);
            
            // Thông báo xóa
            await _notificationHub.Clients.All.SendAsync("NotificationDeleted", new
            {
                notificationId = id,
                timestamp = DateTime.UtcNow
            });

            return Ok();
        }

        /// <summary>
        /// Gửi thông báo trực tiếp tới một user cụ thể
        /// </summary>
        [HttpPost("send-to-user/{accountId}")]
        public async Task<IActionResult> SendNotificationToUser([FromRoute] int accountId, [FromBody] NotificationRequest request)
        {
            // 1. ÉP CỨNG người nhận là accountId này để DB lưu đúng là thông báo cá nhân
            request.listReceiver = new List<ReceiverRequest>
            {
                new ReceiverRequest { Id = accountId }
            };

            // 2. Lưu Database
            await _notificationService.AddNotification(request);

            // 3. Bắn SignalR cho đúng User đó
            await _notificationHub.Clients.User(accountId.ToString()).SendAsync("ReceiveUserNotification", new
            {
                accountId,
                title = request.Title ?? "Thông báo mới",
                message = request.Content,
                timestamp = DateTime.UtcNow,
                isNew = true
            });

            return Ok(new { message = "Thông báo đã được gửi tới user" });
        }
    }
}
