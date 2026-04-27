using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomListResponse>>> GetAllRooms()
        {
            return Ok(await _roomService.GetListRooms());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDetailsResponse>> GetRoomDetails([FromRoute] int id)
        {
            return Ok(await _roomService.GetDetailsRoom(id));
        }

    }
}
