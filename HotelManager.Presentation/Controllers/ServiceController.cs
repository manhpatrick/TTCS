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

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<ServiceCustomerResponse>>> GetServiceByCategory([FromRoute] CategoryService category)
        {
            return Ok(await _serviceService.GetServiceByCategory(category));
        }
    }
}
