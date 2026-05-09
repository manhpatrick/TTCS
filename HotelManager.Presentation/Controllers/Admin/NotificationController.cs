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
        public async Task<IActionResult> AddNotification([FromBody]NotificationRequest request)
        {
            await _notificationService.AddNotification(request);
            
            // Gửi thông báo real-time tới tất cả các user
            await _notificationHub.Clients.All.SendAsync("ReceiveNotification", new
            {
                title = "Thông báo mới",
                message = request.Content,
                timestamp = DateTime.UtcNow,
                isNew = true
            });

            return Ok(new { message = "Thông báo đã được gửi" });
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
            await _notificationService.AddNotification(request);

            // Gửi real-time notification tới user cụ thể
            await _notificationHub.Clients.All.SendAsync("ReceiveUserNotification", new
            {
                accountId,
                title = "Thông báo mới",
                message = request.Content,
                timestamp = DateTime.UtcNow,
                isNew = true
            });

            return Ok(new { message = "Thông báo đã được gửi tới user" });
        }
    }
}
