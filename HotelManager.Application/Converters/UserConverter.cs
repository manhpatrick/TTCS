using HotelManager.Application.DTO.User;
using HotelManager.Domain.Entity.Users;

namespace HotelManager.Application.Converters
{
    public class UserConverter
    {
        public UserInfoResponse EntityToResponse(User user)
        {
            return new UserInfoResponse
            {
                Name = user.Name,
                Gender = user.Gender,
                BirthDay = user.BirthDay,
                Phone = user.Phone,
                Email = user.Email,
                Address = user.Address
            };
        }
    }
}
