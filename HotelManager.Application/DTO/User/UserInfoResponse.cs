

using HotelManager.Domain.Entity.Users.Enum;

namespace HotelManager.Application.DTO.User
{
    public class UserInfoResponse
    {
        public string? Name { get; set; }
        public GenderUser? Gender { get; set; }
        public DateOnly? BirthDay { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
