using HotelManager.Application.DTO.Chatbot;

namespace HotelManager.Application.IService
{
    public interface IChatbotService
    {
        Task<ChatResponse> SendMessage(ChatRequest request);
        Task<ChatResponse> SearchRoomsByQuery(string query);
        Task<ChatResponse> SearchRoomsByPrice(decimal minPrice, decimal maxPrice);
        Task<ChatResponse> SearchRoomsByCategory(string category);
    }
}
