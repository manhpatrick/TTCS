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
        public async Task<ActionResult<IEnumerable<ServiceCustomerResponse>>> GetAllServices()
        {
            return Ok(await _serviceService.GetAllServices());
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ServiceCustomerResponse>>> GetServiceByCategory([FromQuery] CategoryService category)
        {
            var result = await _serviceService.GetServiceByCategory(category);
            return Ok(result);
        }

        // 2. Sắp xếp dịch vụ theo giá
        // Gọi API: GET /api/service/sort-price?isAscending=true (hoặc false)
        [HttpGet("sort-price")]
        public async Task<ActionResult<IEnumerable<ServiceCustomerResponse>>> GetServiceSortPrice([FromQuery] bool isAscending = true)
        {
            var result = await _serviceService.GetServiceSortPrice(isAscending);
            return Ok(result);
        }
    }
}
