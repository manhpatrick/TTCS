using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Services;
using HotelManager.Domain.Entity.Services.Enum;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<Service>> GetServiceByCategory(CategoryService category)
        {
            return await _dbSet.Where(s => s.Category == category).ToListAsync();
        }
    }
}
