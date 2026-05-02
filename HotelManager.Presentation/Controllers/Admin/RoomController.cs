using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers.Admin
{
    public class RoomController : AdminController
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomAdminListResponse>>> GetAllRooms()
        {
            return Ok(await _roomService.GetListRoomsAdmin());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDetailsResponse>> GetRoomDetails([FromRoute] int id)
        {
            return Ok(await _roomService.GetDetailsRoom(id));
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]RoomRequest request)
        {
            await _roomService.Add(request);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] RoomUpdateRequest request)
        {
            await _roomService.Update(id, request);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            await _roomService.Remove(id);
            return Ok();
        }


    }
}
