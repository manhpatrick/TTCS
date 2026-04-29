using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using System.Net;

namespace HotelManager.Infrastructure.Libraries
{
    // 3. Class phụ trợ để lấy IP mạng của người dùng (Bắt buộc gửi cho VNPAY để chống gian lận)
    public static class Utils
    {
        public static string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;
                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }
                    if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();
                    return ipAddress;
                }
            }
            catch (Exception)
            {
                return "127.0.0.1";
            }
            return "127.0.0.1";
        }
    }
}
