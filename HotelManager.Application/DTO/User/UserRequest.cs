
using HotelManager.Domain.Entity.Users.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManager.Application.DTO.User
{
    public class UserRequest
    {
        public string? Name { get; set; }
        [EnumDataType(typeof(GenderUser))]
        public GenderUser? Gender { get; set; }
        public DateOnly? BirthDay { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
