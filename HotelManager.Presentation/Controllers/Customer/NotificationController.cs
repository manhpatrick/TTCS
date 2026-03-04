using HotelManager.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManager.Presentation.Controllers.Customer
{
    public class NotificationController : CustomerController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetNotification()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var list = await _notificationService.GetNotifications(accountId);
            return Ok(list);
        }
        [HttpPut("{notificationid}")]
        public async Task<IActionResult> MarkIsRead([FromRoute]int notificationid)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _notificationService.MarkIsRead(notificationid,accountId);
            return Ok();
        }
    }
}
