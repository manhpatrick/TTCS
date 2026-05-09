using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Accounts;
using HotelManager.Domain.Entity.Accounts.Enum;
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
        public async Task<IEnumerable<Account>> GetAllCustomerAccounts()
        {
            return await _dbSet.Include(a => a.User).Where(a => a.Role == RoleAccount.Customer).OrderBy(a => a.Id).ToListAsync();
        }
    }
}
