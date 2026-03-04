using HotelManager.Application.DTO.Bookings;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManager.Presentation.Controllers.Customer
{
    public class BookingController : CustomerController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingRequest request)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _bookingService.AddBooking(accountId, request);
            return Ok();
        }
        [HttpGet("me")]
        public async Task<ActionResult<BookingDetailResponse>> GetBookingsByAccountId()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _bookingService.GetBookingsByAccountId(accountId));
        }
    }
}
