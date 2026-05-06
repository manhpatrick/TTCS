using HotelManager.Application.DTO;
using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.IService;
using HotelManager.Domain.Entity.Rooms.Enum;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IRatingService _ratingService;
        public RoomController(IRoomService roomService, IRatingService ratingService)
        {
            _roomService = roomService;
            _ratingService = ratingService;
        }


        [HttpGet]
        public async Task<ActionResult<PagedResponse<RoomUserListResponse>>> GetAllRooms([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 6)
        {
            return Ok(await _roomService.GetListRooms(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDetailsResponse>> GetRoomDetails([FromRoute] int id)
        {
            return Ok(await _roomService.GetDetailsRoom(id));
        }

        [HttpGet("{id}/ratings")]
        public async Task<ActionResult<IEnumerable<RatingResponse>>> GetRoomRatings([FromRoute] int id)
        {
            return Ok(await _ratingService.GetRoomRatings(id));
        }

        [HttpGet("{id}/available-times")]
        public async Task<ActionResult<List<AvailableTimeResponse>>> GetAvailableTimes(
            [FromRoute] int id,
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to)
        {
            var availableTimes = await _roomService.CalculateAvailableTimes(id, from, to);
            return Ok(availableTimes);
        }

        [HttpGet("search")] // Đổi route thành /api/room/search
        public async Task<ActionResult<PagedResponse<RoomUserListResponse>>> SearchRooms(
            [FromQuery] CategoryRoom? category = null,
            [FromQuery] bool? isAscending = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6)
        {
            var result = await _roomService.GetRoomsAdvanced(category, isAscending, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
