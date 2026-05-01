namespace HotelManager.Application.DTO.Feedbacks
{
    public class FeedbackAdminResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string SenderName { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhone { get; set; }
        public string ShortContent => Content.Length > 20 ? Content.Substring(0, 20) + "..." : Content;
    }
}
