
using HotelManager.Application.DTO.User;

namespace HotelManager.Application.IService
{
    public interface IUserService
    {
        Task UpdateUserInfo(int accountId, UserRequest request);
        Task<UserInfoResponse> GetUserInfo(int accountId);
    }
}
