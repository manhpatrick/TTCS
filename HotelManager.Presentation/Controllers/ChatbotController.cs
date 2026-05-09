using HotelManager.Application.DTO.Chatbot;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        /// <summary>
        /// Gửi tin nhắn đến chatbot. Chatbot sẽ phân tích và trả lời hoặc gợi ý phòng phù hợp.
        /// </summary>
        /// <remarks>
        /// Ví dụ:
        /// - "Tôi muốn tìm phòng giá từ 500k đến 1 triệu"
        /// - "Có phòng Deluxe nào không?"
        /// - "Tìm phòng trong khoảng 2 triệu đến 5 triệu"
        /// </remarks>
        [HttpPost("message")]
        public async Task<ActionResult<ChatResponse>> SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { message = "Tin nhắn không được để trống" });
            }

            var response = await _chatbotService.SendMessage(request);
            return Ok(response);
        }

        /// <summary>
        /// Tìm phòng theo khoảng giá
        /// </summary>
        [HttpGet("search/by-price")]
        public async Task<ActionResult<ChatResponse>> SearchByPrice(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return BadRequest(new { message = "Khoảng giá không hợp lệ" });
            }

            var response = await _chatbotService.SearchRoomsByPrice(minPrice, maxPrice);
            return Ok(response);
        }

        /// <summary>
        /// Tìm phòng theo loại/hạng (Standard, Deluxe, Suite, Presidential)
        /// </summary>
        [HttpGet("search/by-category")]
        public async Task<ActionResult<ChatResponse>> SearchByCategory([FromQuery] string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest(new { message = "Loại phòng không được để trống" });
            }

            var response = await _chatbotService.SearchRoomsByCategory(category);
            return Ok(response);
        }

        /// <summary>
        /// Tìm phòng theo tìm kiếm tự do
        /// </summary>
        [HttpPost("search")]
        public async Task<ActionResult<ChatResponse>> SearchRooms([FromBody] ChatRequest request)
        {
            var response = await _chatbotService.SearchRoomsByQuery(request.Message);
            return Ok(response);
        }
    }
}
