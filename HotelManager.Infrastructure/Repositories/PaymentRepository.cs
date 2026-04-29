using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Payments;
using HotelManager.Infrastructure.Data;

namespace HotelManager.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }
    }
}
