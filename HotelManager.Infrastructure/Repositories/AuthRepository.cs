using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Accounts;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class AuthRepository : GenericRepository<Account>, IAuthRepository
    {
        public AuthRepository(AppDbContext context) : base(context) { }

        public async Task<Account?> GetAccountByUsername(string username)
        {
            var exist =  await _dbSet.Include(a => a.User).FirstOrDefaultAsync(a => a.Username == username);
            return exist;
        }
    }
}
