using HotelManager.Domain.Entity.Services;
using HotelManager.Domain.Entity.Services.Enum;

namespace HotelManager.Application.IRepository
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<IEnumerable<Service>> GetServiceByCategory(CategoryService category);
    }
}
