using HotelManager.Application.Converters;
using HotelManager.Application.DTO;
using HotelManager.Application.DTO.Services;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;
using HotelManager.Domain.Entity.Services.Enum;

namespace HotelManager.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ServiceConverter _serviceConverter;

        public ServiceService(IServiceRepository serviceRepository, ServiceConverter serviceConverter)
        {
            _serviceRepository = serviceRepository;
            _serviceConverter = serviceConverter;
        }

        public async Task Add(ServiceRequest request)
        {
            await _serviceRepository.Add(_serviceConverter.DtoToEntity(request));
        }

        public async Task<IEnumerable<ServiceAdminResponse>> GetAllServicesAdmin()
        {
            var lists = await _serviceRepository.GetAll();
            return lists.Select(service => _serviceConverter.EntityToDtoAdmin(service));
        }
        public async Task<PagedResponse<ServiceCustomerResponse>> GetAllServices(int pageNumber = 1, int pageSize = 6)
        {
            var lists = await _serviceRepository.GetListsService();
            var totalRecords = lists.Count();
            var pagedData = lists.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .Select(service => _serviceConverter.EntityToDto(service))
                                .ToList();
            return new PagedResponse<ServiceCustomerResponse>(pagedData, pageNumber, pageSize, totalRecords);
        }

        public async Task<PagedResponse<ServiceCustomerResponse>> GetServicesAdvanced(CategoryService? category, bool? isAscending, int pageNumber = 1, int pageSize = 6)
        {
            var allServices = await _serviceRepository.GetListsService(); // Hoặc lấy dạng IQueryable
            var query = allServices.AsQueryable();

            if (category.HasValue)
                query = query.Where(s => s.Category == category.Value);

            if (isAscending.HasValue)
                query = isAscending.Value
                    ? query.OrderBy(s => s.Price)
                    : query.OrderByDescending(s => s.Price);

            var totalRecords = query.Count();

            var pagedData = query.Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .Select(service => _serviceConverter.EntityToDto(service))
                                 .ToList();

            return new PagedResponse<ServiceCustomerResponse>(pagedData, pageNumber, pageSize, totalRecords);
        }

        public async Task Remove(int id)
        {
            var service =  await _serviceRepository.GetById(id);
            await _serviceRepository.Remove(service);
        }

        public async Task Update(int id, ServiceUpdateRequest request)
        {
            var service = await _serviceRepository.GetById(id);
            if(request.Name != null) service.ChangeName(request.Name);
            if (request.Category != null) service.ChangeCategoryService(request.Category.Value);
            if (request.Price != null) service.ChangePrice(request.Price.Value);
            if (request.Unit != null) service.ChangeUnit(request.Unit);
            if (request.IsActive != null) service.ChangeIsActive(request.IsActive.Value);
            if (request.ImageUrl != null) service.ChangeImage(request.ImageUrl);
            await _serviceRepository.SaveAsync();
        }
    }
}
