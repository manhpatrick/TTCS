using HotelManager.Application.DTO.Account;
using HotelManager.Domain.Entity.Accounts;

namespace HotelManager.Application.Converters
{
    public class AccountConverter
    {
        public ListAccountsResponse entityToDto(Account account)
        {
            return new ListAccountsResponse
            {
                Id = account.Id,
                Username = account.Username,
                Phone = account.User?.Phone,
                Email = account.User?.Email,
                FullName = account.User?.Name,
                Gender = account.User?.Gender,
                Birthday = account.User?.BirthDay,
                Address = account.User?.Address,
                IsActive = account.IsActive,
                Role = (int)account.Role
            };
        }
    }
}
