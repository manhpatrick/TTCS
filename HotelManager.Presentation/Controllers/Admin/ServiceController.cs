using HotelManager.Application.DTO.Services;
using HotelManager.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace HotelManager.Presentation.Controllers.Admin
{
    public class ServiceController : AdminController
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceAdminResponse>>> GetAllServices()
        {
            return Ok(await _serviceService.GetAllServicesAdmin());
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]ServiceRequest request)
        {
            await _serviceService.Add(request);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] ServiceUpdateRequest request)
        {
            await _serviceService.Update(id, request);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _serviceService.Remove(id);
            return Ok();
        }
    }
}
