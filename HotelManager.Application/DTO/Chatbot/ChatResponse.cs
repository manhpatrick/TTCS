namespace HotelManager.Application.DTO.Chatbot
{
    public class ChatResponse
    {
        public string Message { get; set; } = string.Empty;
        public List<RoomSuggestion>? Rooms { get; set; }
        public bool IsRoomQuery { get; set; }
        public string Type { get; set; } = "text"; // text, room_suggestion, error
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class RoomSuggestion
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
