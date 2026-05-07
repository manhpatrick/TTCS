using HotelManager.Application.DTO;
using HotelManager.Application.DTO.Services;
using HotelManager.Application.IService;
using HotelManager.Domain.Entity.Services.Enum;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ServiceCustomerResponse>>> GetAllServices([FromQuery] int pageNumber = 1,
                                                                                              [FromQuery] int pageSize = 6)
        {
            return Ok(await _serviceService.GetAllServices(pageNumber, pageSize));
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResponse<ServiceCustomerResponse>>> SearchServices(
            [FromQuery] CategoryService? category = null,
            [FromQuery] bool? isAscending = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6)
        {
            return Ok(await _serviceService.GetServicesAdvanced(category, isAscending, pageNumber, pageSize));
        }
    }
}
