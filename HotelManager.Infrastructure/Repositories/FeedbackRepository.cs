using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Feedbacks;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<Feedback>> GetFeedbacksAdmin()
        {
            return await _dbSet.Include(f => f.Account)
                .ThenInclude(a => a.User)
                .OrderByDescending(f => f.CreatedAt).ToListAsync();
        }
    }
}
