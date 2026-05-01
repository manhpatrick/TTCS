using HotelManager.Domain.Entity.Accounts;
using HotelManager.Domain.Exceptions;

namespace HotelManager.Domain.Entity.Feedbacks
{
    public class Feedback
    {
        protected Feedback() { }
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsRead { get; private set; }
        public int AccountId { get; private set; }
        public Account? Account { get; private set; }

        public Feedback(string title, string content, int accountId)
        {
            ChangeTitle(title);
            ChangeContent(content);
            CreatedAt = DateTime.UtcNow;
            IsRead = false;
            AccountId = accountId;
        }
        public void ChangeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new DomainException("Tiêu đề không được để trống");
            Title = title;
        }
        public void ChangeContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new DomainException("Nội dung không được để trống");
            Content = content;
        }
        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
