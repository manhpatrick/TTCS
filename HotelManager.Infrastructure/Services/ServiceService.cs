using HotelManager.Application.Converters;
using HotelManager.Application.DTO.Rooms;
using HotelManager.Application.DTO.Services;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;
using HotelManager.Domain.Entity.Rooms.Enum;
using HotelManager.Domain.Entity.Services;
using HotelManager.Domain.Entity.Services.Enum;
using HotelManager.Infrastructure.Repositories;

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
        public async Task<IEnumerable<ServiceCustomerResponse>> GetAllServices()
        {
            var lists = await _serviceRepository.GetListsService();
            return lists.Select(service => _serviceConverter.EntityToDto(service));
        }

        public async Task<IEnumerable<ServiceCustomerResponse>> GetServiceByCategory(CategoryService category)
        {
            var lists = await _serviceRepository.GetServiceByCategory(category);
            return lists.Select(service => _serviceConverter.EntityToDto(service));
        }
        public async Task<IEnumerable<ServiceCustomerResponse>> GetServiceSortPrice(bool isAscending = true)
        {
            var lists = await _serviceRepository.GetServiceSortedByPrice(isAscending);
            return lists.Select(service => _serviceConverter.EntityToDto(service));
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
