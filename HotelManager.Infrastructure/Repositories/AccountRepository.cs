using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Accounts;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _dbSet.Include(a => a.User).OrderBy(a => a.Username).ToListAsync();
        }
    }
}
