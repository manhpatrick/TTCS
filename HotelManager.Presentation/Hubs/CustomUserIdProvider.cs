using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HotelManager.Presentation.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            // Bảo SignalR lấy giá trị của claim "sub" làm UserIdentifier
            return connection.User?.FindFirst("sub")?.Value
                ?? connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}