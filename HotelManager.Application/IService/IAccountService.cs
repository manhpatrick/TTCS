using HotelManager.Application.DTO.Account;

namespace HotelManager.Application.IService
{
    public interface IAccountService
    {
        Task<IEnumerable<ListAccountsResponse>> GetAllAccounts();
        Task UpdateIsActive(int id, bool isActive);
    }
}
