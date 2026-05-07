using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Payments;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetPaymentsByAccountId(int accountId)
        {
            return await _dbSet.Include(p => p.Booking)
                                .Where(p => p.Booking.AccountId == accountId).ToListAsync();
        }
    }
}
