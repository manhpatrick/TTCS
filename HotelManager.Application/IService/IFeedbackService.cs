using HotelManager.Application.DTO.Feedbacks;

namespace HotelManager.Application.IService
{
    public interface IFeedbackService
    {
        Task AddFeedback(FeedbackRequest request, int accountId);
        Task DeleteFeedback(int id);
        Task<IEnumerable<FeedbackAdminResponse>> GetFeedbacksAdmin();
        Task MarkAsRead(int id);
    }
}
