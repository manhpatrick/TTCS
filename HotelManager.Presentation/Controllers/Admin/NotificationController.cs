using HotelManager.Application.DTO.Notifications;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers.Admin
{
    public class NotificationController : AdminController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationAdminResponse>>> GetNotificationAdmin()
        {
            return Ok(await _notificationService.GetNotificationsAdmin());
        }
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody]NotificationRequest request)
        {
            await _notificationService.AddNotification(request);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification([FromRoute]int id, [FromBody] NotificationUpdateRequest request)
        {
            await _notificationService.UpdateNotification(id, request);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] int id)
        {
            await _notificationService.DeleteNotification(id);
            return Ok();
        }
    }
}
