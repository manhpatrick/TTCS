using HotelManager.Domain.Entity.Feedbacks;

namespace HotelManager.Application.IRepository
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetFeedbacksAdmin();
    }
}
