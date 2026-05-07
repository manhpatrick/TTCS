using HotelManager.Application.DTO.Ratings;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManager.Presentation.Controllers.Customer
{
    public class RatingController : CustomerController
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] RatingRequest request)
        {
            // Lấy ID người dùng đang đăng nhập từ Token
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _ratingService.AddRating(accountId, request);

            return Ok();
        }
    }
}
