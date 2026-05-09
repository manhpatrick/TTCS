using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Payment.Enum;
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
        public async Task<IEnumerable<Payment>> GetListPaymentSuccess()
        {
            return await _dbSet.Include(p => p.Booking)
                               .ThenInclude(b => b.Account)
                               .ThenInclude(a => a.User)
                               .OrderByDescending(p => p.PaymentDate)
                               .Where(p => p.Status == PaymentStatus.Success).ToListAsync();
        }

    }
}
