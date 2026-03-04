
namespace HotelManager.Application.DTO.AppConfig
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string ExpireMinutes { get; set; }
    }
}
