using HotelManager.Application.DTO.Feedbacks;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers.Admin
{
    public class FeedbackController : AdminController
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackAdminResponse>>> GetFeedbacksAdmin()
        {
            return Ok(await _feedbackService.GetFeedbacksAdmin());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> MarkAsRead([FromRoute] int id)
        {
            await _feedbackService.MarkAsRead(id);
            return Ok();
        }
    }
}
