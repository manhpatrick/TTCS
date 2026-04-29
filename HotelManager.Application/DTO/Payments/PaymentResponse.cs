namespace HotelManager.Application.DTO.Payments
{
    public class PaymentResponse
    {
        // Trạng thái thanh toán: true nếu thành công, false nếu thất bại hoặc bị hủy
        public bool Success { get; set; }

        // Phương thức thanh toán (ví dụ: "VnPay")
        public string PaymentMethod { get; set; }

        // Thông tin mô tả đơn hàng
        public string OrderDescription { get; set; }

        // Mã đơn hàng của bạn (BookingId hoặc chuỗi tham chiếu lúc bạn gửi đi)
        public string OrderId { get; set; }

        // Mã giao dịch ghi nhận trên hệ thống của VNPAY (vnp_TransactionNo)
        // Rất quan trọng: Dùng mã này để lưu vào Entity Payment giúp đối soát sau này
        public string TransactionId { get; set; }

        // Chữ ký bảo mật (vnp_SecureHash)
        public string Token { get; set; }

        // Mã lỗi chi tiết từ VNPAY (Ví dụ: "00" là thành công, "24" là khách hủy, "51" là số dư không đủ...)
        public string VnPayResponseCode { get; set; }
    }
}
