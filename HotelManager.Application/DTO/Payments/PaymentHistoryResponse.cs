namespace HotelManager.Application.DTO.Payments
{
    public class PaymentHistoryResponse
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; } // 1: Thành công, 2: Thất bại, 3: Đang chờ
    }
}