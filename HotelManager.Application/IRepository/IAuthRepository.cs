using HotelManager.Application.DTO.Auth;
using HotelManager.Domain.Entity.Accounts;

namespace HotelManager.Application.IRepository
{
    public interface IAuthRepository : IGenericRepository<Account>
    {
        Task<Account> GetAccountByUsername(string Username);

    }
}
