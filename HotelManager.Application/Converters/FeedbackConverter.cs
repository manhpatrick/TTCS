using HotelManager.Application.DTO.Feedbacks;
using HotelManager.Domain.Entity.Feedbacks;

namespace HotelManager.Application.Converters
{
    public class FeedbackConverter
    {
        public FeedbackAdminResponse EntityToDtoAdmin(Feedback feedback)
        {
            return new FeedbackAdminResponse
            {
                Id = feedback.Id,
                Title = feedback.Title,
                Content = feedback.Content,
                CreatedAt = feedback.CreatedAt,
                IsRead = feedback.IsRead,
                AccountId = feedback.AccountId,
                SenderName = feedback.Account?.User?.Name ?? "Người dùng ẩn danh",
                SenderPhone = feedback.Account?.User?.Phone ?? "Người dùng ẩn danh",
                SenderUsername = feedback.Account?.Username ?? "Người dùng ẩn danh"
            };
        }

        public Feedback DtoToEntity(FeedbackRequest request, int accountId)
        {
            return new Feedback(request.Title, request.Content, accountId);
        }
    }
}
