using HotelManager.Application.DTO.Notifications;
using HotelManager.Domain.Entity.Accounts;

namespace HotelManager.Application.IRepository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<IEnumerable<Account>> GetAllCustomerAccounts();
    }
}
