using HotelManager.Application.DTO.Payments;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers.Admin
{
    [ApiController]
    public class PaymentController : AdminController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistoryAdminResponse>>> GetAllPaymentSuccess()
        {
            return Ok(await _paymentService.GetListPaymentsSuccess());
        }
    }
}
