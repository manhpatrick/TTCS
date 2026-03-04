using HotelManager.Application.Converters;
using HotelManager.Application.DTO.User;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserConverter _userConverter;

        public UserService(IUserRepository userRepository, UserConverter userConverter)
        {
            _userRepository = userRepository;
            _userConverter = userConverter;
        }

        public async Task UpdateUserInfo(int accountId, UserRequest request)
        {
            var user = await _userRepository.GetUserByAccountId(accountId);
            if (request.Name != null) user.ChangeName(request.Name);
            if (request.Gender != null) user.ChangeGender(request.Gender.Value);
            if (request.BirthDay != null) user.ChangeBirthDay(request.BirthDay.Value);
            if (request.Phone != null) user.ChangePhone(request.Phone);
            if (request.Email != null) user.ChangeEmail(request.Email);
            if (request.Address != null) user.ChangeAddress(request.Address);
            await _userRepository.SaveAsync();
        }
        public async Task<UserInfoResponse> GetUserInfo(int accountId)
        {
            return _userConverter.EntityToResponse(await _userRepository.GetUserByAccountId(accountId));
        }
    }
}
