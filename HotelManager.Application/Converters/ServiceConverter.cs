using HotelManager.Application.DTO.Services;
using HotelManager.Domain.Entity.Services;

namespace HotelManager.Application.Converters
{
    public class ServiceConverter
    {
        public Service DtoToEntity(ServiceRequest request)
        {
            return new Service(request.Name, request.Price, request.Unit, request.Category, request.ImageUrl);
        }
        public ServiceAdminResponse EntityToDtoAdmin(Service service)
        {
            return new ServiceAdminResponse
            {
                Id = service.Id,
                Name = service.Name,
                Category = service.Category,
                Price = service.Price,
                Unit = service.Unit,
                IsActive = service.IsActive,
                ImageUrl = service.ImageUrl,
                Description = service.Description,
            };
        }
        public ServiceCustomerResponse EntityToDto(Service service)
        {
            return new ServiceCustomerResponse
            {
                Id = service.Id,
                Name = service.Name,
                Category = service.Category,
                Price = service.Price,
                Unit = service.Unit,
                ImageUrl = service.ImageUrl,
                Description = service.Description,
            };
        }
    }
}
