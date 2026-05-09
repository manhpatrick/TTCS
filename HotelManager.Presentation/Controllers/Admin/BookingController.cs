using HotelManager.Application.DTO.Bookings;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers.Admin
{
    public class BookingController : AdminController
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDetailAdminResponse>>> GetAllBookings()
        {
            return Ok(await _bookingService.GetListBookings());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking([FromRoute] int id, [FromBody]BookingUpdateRequest request)
        {
            await _bookingService.Update(id, request);
            return Ok();
        }
    }
}
