namespace HotelManager.Application.DTO.Payments
{
    public class PaymentInformationRequest
    {
        public string OrderType { get; set; } // Thường để là "other"
        public decimal Amount { get; set; } // Tổng tiền cần thanh toán
        public string OrderDescription { get; set; } // Mô tả (VD: "Thanh toán phòng VIP")
        public string Name { get; set; } // Tên khách hàng
        public string TxnRef { get; set; }
    }
}
