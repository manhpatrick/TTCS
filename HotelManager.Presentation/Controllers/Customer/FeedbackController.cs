using HotelManager.Application.DTO.Feedbacks;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManager.Presentation.Controllers.Customer
{
    public class FeedbackController : CustomerController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _feedbackService.AddFeedback(feedbackRequest, accountId);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] int id)
        {
            await _feedbackService.DeleteFeedback(id);
            return Ok();
        }
    }
}
