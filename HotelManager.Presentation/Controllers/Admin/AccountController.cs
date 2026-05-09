using HotelManager.Application.DTO.Account;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers.Admin
{
    public class AccountController : AdminController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListAccountsResponse>>> GetAllAccounts()
        {
            return Ok(await _accountService.GetAllAccounts());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIsActive([FromRoute] int id, [FromBody] bool isActive)
        {
            await _accountService.UpdateIsActive(id, isActive);
            return Ok();
        }
    }
}
