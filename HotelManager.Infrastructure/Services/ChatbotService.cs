using HotelManager.Application.DTO.Chatbot;
using HotelManager.Application.IRepository;
using HotelManager.Application.IService;
using HotelManager.Domain.Entity.Rooms.Enum;
using System.Text.RegularExpressions;

namespace HotelManager.Infrastructure.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly IRoomRepository _roomRepository;

        public ChatbotService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<ChatResponse> SendMessage(ChatRequest request)
        {
            var message = request.Message.ToLower().Trim();

            // Phân tích yêu cầu của user
            if (ContainsKeywords(message, new[] { "phòng", "room", "tìm", "search", "giá", "price", "hạng", "category" }))
            {
                // Trích xuất giá từ tin nhắn
                var prices = ExtractPrices(message);
                var category = ExtractCategory(message);

                if (prices.HasValue)
                {
                    return await SearchRoomsByPrice(prices.Value.Min, prices.Value.Max);
                }
                else if (!string.IsNullOrEmpty(category))
                {
                    return await SearchRoomsByCategory(category);
                }
                else
                {
                    return await SearchRoomsByQuery(message);
                }
            }

            // Phản hồi cho các câu hỏi thông thường
            return GenerateDefaultResponse(message);
        }

        public async Task<ChatResponse> SearchRoomsByQuery(string query)
        {
            try
            {
                var rooms = await _roomRepository.GetAll();
                var availableRooms = rooms.Where(r => r.RoomStatus == RoomStatus.Available).ToList();

                if (!availableRooms.Any())
                {
                    return new ChatResponse
                    {
                        Message = "Xin lỗi, hiện tại không có phòng nào trống. Vui lòng quay lại sau!",
                        IsRoomQuery = true,
                        Type = "text"
                    };
                }

                var suggestions = availableRooms.Take(5).Select(r => new RoomSuggestion
                {
                    Id = r.Id,
                    Name = r.Name,
                    Price = r.PricePerNight,
                    Category = r.Category.ToString(),
                    Capacity = r.Capacity,
                    IsAvailable = r.RoomStatus == RoomStatus.Available
                }).ToList();

                return new ChatResponse
                {
                    Message = $"Tôi tìm thấy {suggestions.Count} phòng có sẵn cho bạn:",
                    Rooms = suggestions,
                    IsRoomQuery = true,
                    Type = "room_suggestion"
                };
            }
            catch (Exception ex)
            {
                return new ChatResponse
                {
                    Message = $"Có lỗi khi tìm kiếm phòng: {ex.Message}",
                    IsRoomQuery = false,
                    Type = "error"
                };
            }
        }

        public async Task<ChatResponse> SearchRoomsByPrice(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var rooms = await _roomRepository.GetAll();
                var filtered = rooms.Where(r => r.RoomStatus == RoomStatus.Available && r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice)
                    .OrderBy(r => r.PricePerNight)
                    .Take(6)
                    .Select(r => new RoomSuggestion
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Price = r.PricePerNight,
                        Category = r.Category.ToString(),
                        Capacity = r.Capacity,
                        IsAvailable = r.RoomStatus == RoomStatus.Available
                    })
                    .ToList();

                if (!filtered.Any())
                {
                    return new ChatResponse
                    {
                        Message = $"Không tìm thấy phòng trong khoảng giá {minPrice:N0}đ - {maxPrice:N0}đ. Bạn có muốn tìm kiếm theo khoảng giá khác không?",
                        IsRoomQuery = true,
                        Type = "text"
                    };
                }

                return new ChatResponse
                {
                    Message = $"Tôi tìm thấy {filtered.Count} phòng trong khoảng giá {minPrice:N0}đ - {maxPrice:N0}đ:",
                    Rooms = filtered,
                    IsRoomQuery = true,
                    Type = "room_suggestion"
                };
            }
            catch (Exception ex)
            {
                return new ChatResponse
                {
                    Message = $"Có lỗi khi tìm kiếm: {ex.Message}",
                    IsRoomQuery = false,
                    Type = "error"
                };
            }
        }

        public async Task<ChatResponse> SearchRoomsByCategory(string category)
        {
            try
            {
                var rooms = await _roomRepository.GetAll();
                
                // Parse category string to enum
                if (!Enum.TryParse<CategoryRoom>(category, true, out var categoryEnum))
                {
                    return new ChatResponse
                    {
                        Message = "Tôi không hiểu loại phòng bạn tìm. Vui lòng thử lại với: Standard, Deluxe, Suite, hoặc Presidential.",
                        IsRoomQuery = true,
                        Type = "text"
                    };
                }

                var filtered = rooms.Where(r => r.RoomStatus == RoomStatus.Available && r.Category == categoryEnum)
                    .OrderBy(r => r.PricePerNight)
                    .Take(6)
                    .Select(r => new RoomSuggestion
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Price = r.PricePerNight,
                        Category = r.Category.ToString(),
                        Capacity = r.Capacity,
                        IsAvailable = r.RoomStatus == RoomStatus.Available
                    })
                    .ToList();

                if (!filtered.Any())
                {
                    return new ChatResponse
                    {
                        Message = $"Xin lỗi, hiện tại không có phòng {category} nào trống.",
                        IsRoomQuery = true,
                        Type = "text"
                    };
                }

                return new ChatResponse
                {
                    Message = $"Tôi tìm thấy {filtered.Count} phòng {category}:",
                    Rooms = filtered,
                    IsRoomQuery = true,
                    Type = "room_suggestion"
                };
            }
            catch (Exception ex)
            {
                return new ChatResponse
                {
                    Message = $"Có lỗi khi tìm kiếm: {ex.Message}",
                    IsRoomQuery = false,
                    Type = "error"
                };
            }
        }

        // Helper methods
        private bool ContainsKeywords(string text, string[] keywords)
        {
            return keywords.Any(k => text.Contains(k));
        }

        private (decimal Min, decimal Max)? ExtractPrices(string text)
        {
            // Tìm kiếm các mẫu như "1 triệu đến 2 triệu", "100k đến 200k", "1000000 đến 2000000"
            var pricePattern = @"(\d+)\s*(?:triệu|k|000000)?.*(?:đến|to|-)\s*(\d+)\s*(?:triệu|k|000000)?";
            var match = Regex.Match(text, pricePattern);

            if (match.Success)
            {
                if (decimal.TryParse(match.Groups[1].Value, out var min) && decimal.TryParse(match.Groups[2].Value, out var max))
                {
                    // Chuyển đổi nếu cần (ví dụ: 1 triệu = 1,000,000)
                    if (text.Contains("triệu"))
                    {
                        min *= 1000000;
                        max *= 1000000;
                    }
                    else if (text.Contains("k"))
                    {
                        min *= 1000;
                        max *= 1000;
                    }

                    return (min, max);
                }
            }

            return null;
        }

        private string? ExtractCategory(string text)
        {
            var categories = new Dictionary<string, string>
            {
                { "standard", "Standard" },
                { "deluxe", "Deluxe" },
                { "suite", "Suite" },
                { "presidential", "Presidential" },
                { "vip", "Presidential" }
            };

            foreach (var category in categories)
            {
                if (text.Contains(category.Key))
                {
                    return category.Value;
                }
            }

            return null;
        }

        private ChatResponse GenerateDefaultResponse(string message)
        {
            var responses = new[]
            {
                "Bạn muốn tìm phòng nào? Tôi có thể giúp bạn tìm theo giá hoặc loại phòng!",
                "Mô tả yêu cầu của bạn chi tiết hơn - bạn muốn tìm phòng trong khoảng giá bao nhiêu?",
                "Tôi là trợ lý của khách sạn. Bạn có thể hỏi tôi về các phòng có sẵn, giá cả, hoặc loại phòng."
            };

            return new ChatResponse
            {
                Message = responses[new Random().Next(responses.Length)],
                IsRoomQuery = false,
                Type = "text"
            };
        }
    }
}
