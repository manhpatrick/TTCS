using HotelManager.Application.DTO;
using HotelManager.Application.DTO.Services;
using HotelManager.Domain.Entity.Services.Enum;

namespace HotelManager.Application.IService
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceAdminResponse>> GetAllServicesAdmin();
        Task<PagedResponse<ServiceCustomerResponse>> GetAllServices(int pageNumber = 1, int pageSize = 6);
        Task<PagedResponse<ServiceCustomerResponse>> GetServicesAdvanced(CategoryService? category, bool? isAscending, int pageNumber = 1, int pageSize = 6);
        Task Add(ServiceRequest request);
        Task Update(int id, ServiceUpdateRequest request);
        Task Remove(int id);

    }
}
