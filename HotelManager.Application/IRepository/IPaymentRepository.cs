using HotelManager.Domain.Entity.Payments;

namespace HotelManager.Application.IRepository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByAccountId(int accountId);
        Task<IEnumerable<Payment>> GetListPaymentSuccess();
    }
}
