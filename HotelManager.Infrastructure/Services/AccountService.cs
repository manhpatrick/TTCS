using HotelManager.Application.Converters;
using HotelManager.Application.CustomException;
using HotelManager.Application.DTO.Account;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AccountConverter _accountConverter;
        public AccountService(IAccountRepository accountRepository, AccountConverter accountConverter)
        {
            _accountRepository = accountRepository;
            _accountConverter = accountConverter;
        }

        public async Task<IEnumerable<ListAccountsResponse>> GetAllAccounts()
        {
            var lists = await _accountRepository.GetAllAccounts();
            return lists.Select(a => _accountConverter.entityToDto(a));
        }
        public async Task UpdateIsActive(int id, bool isActive)
        {
            var exist = await _accountRepository.GetById(id);
            if (exist == null) throw new NotExistsException("Tài khoản không tồn tại");
            exist.ChangeIsActive(isActive);
        }
    }
}
