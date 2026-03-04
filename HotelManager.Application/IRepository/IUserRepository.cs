using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Users;

namespace HotelManager.Application.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByAccountId(int accountId);
    }
}
