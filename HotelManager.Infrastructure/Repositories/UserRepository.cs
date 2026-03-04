using HotelManager.Application.CustomException.Auth;
using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Users;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
        public async Task<User> GetUserByAccountId(int accountId)
        {
            var user =  await _dbSet.FirstOrDefaultAsync(u => u.AccountId == accountId);
            if (user == null) throw new UsernameNotExistException("Không tồn tại thông tin tài khoản");
            return user;
        }
    }
}
