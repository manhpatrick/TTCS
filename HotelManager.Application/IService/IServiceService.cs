
using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.DTO.Services;
using HotelManager.Domain.Entity.Services.Enum;

namespace HotelManager.Application.IService
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceAdminResponse>> GetAllServicesAdmin();
        Task<IEnumerable<ServiceCustomerResponse>> GetAllServices();
        Task<IEnumerable<ServiceCustomerResponse>> GetServiceByCategory(CategoryService category);
        Task Add(ServiceRequest request);
        Task Update(int id, ServiceUpdateRequest request);
        Task Remove(int id);

    }
}
