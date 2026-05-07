using HotelManager.Application.DTO.Payments;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManager.Presentation.Controllers.Customer
{
    public class PaymentController : CustomerController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // 1. Bắn link về cho Frontend
        [HttpPost("vnpay/{bookingId}")]
        public async Task<IActionResult> CreatePaymentUrl(int bookingId)
        {
            var url = await _paymentService.CreateVnPayUrlAsync(bookingId, HttpContext);
            return Ok(url);
        }

        // 2. Nhận Callback từ VNPAY
        [AllowAnonymous]
        [HttpGet("vnpay-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = await _paymentService.ProcessVnPayCallbackAsync(HttpContext.Request.Query);

            if (response.Success)
            {
                // Trả về trang cảm ơn
                return Redirect("/booking-success.html");
            }

            // Trả về trang lỗi
            return Redirect("/booking-failed.html");
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<PaymentHistoryResponse>>> GetUserPayments()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _paymentService.GetUserPayments(accountId));
        }
    }
}
