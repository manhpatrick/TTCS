namespace HotelManager.Application.DTO.Chatbot
{
    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public int? AccountId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? RoomCategory { get; set; }
    }
}
