using HotelManager.Application.Converters;
using HotelManager.Application.CustomException;
using HotelManager.Application.DTO.Feedbacks;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;

namespace HotelManager.Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly FeedbackConverter _feedbackConverter;

        public FeedbackService(IFeedbackRepository feedbackRepository, FeedbackConverter feedbackConverter)
        {
            _feedbackRepository = feedbackRepository;
            _feedbackConverter = feedbackConverter;
        }

        public async Task AddFeedback(FeedbackRequest request, int accountId)
        {
            await _feedbackRepository.Add(_feedbackConverter.DtoToEntity(request, accountId));
        }

        public async Task DeleteFeedback(int id)
        {
            var feedback = await _feedbackRepository.GetById(id);
            if (feedback == null) throw new NotExistsException("Phản hồi không tồn tại");
            await _feedbackRepository.Remove(feedback);
        }

        public async Task<IEnumerable<FeedbackAdminResponse>> GetFeedbacksAdmin()
        {
            var lists = await _feedbackRepository.GetFeedbacksAdmin();
            return lists.Select(f => _feedbackConverter.EntityToDtoAdmin(f));
        }
        public async Task MarkAsRead(int id)
        {
            var feedback = await _feedbackRepository.GetById(id);
            if (feedback == null) throw new NotExistsException("Phản hồi không tồn tại");
            feedback.MarkAsRead();
            await _feedbackRepository.SaveAsync();
        }
    }
}
