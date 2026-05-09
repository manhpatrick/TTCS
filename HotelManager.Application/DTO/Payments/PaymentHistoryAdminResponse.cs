namespace HotelManager.Application.DTO.Payments
{
    public class PaymentHistoryAdminResponse
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int Status { get; set; } // 1: Thành công, 2: Thất bại, 3: Đang chờ
    }
}