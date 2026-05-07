using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Entity.Payment.Enum;

namespace HotelManager.Domain.Entity.Payments
{
    public class Payment
    {
        public int Id { get; private set; }
        public int BookingId { get; private set; }
        public Booking Booking { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string PaymentMethod { get; private set; } // "VNPAY", "Tiền mặt"

        // Thêm các trường này để lưu thông tin từ VNPAY
        public string? ExternalTransactionId { get; private set; } // vnp_TransactionNo
        public string? OrderInfo { get; private set; }            // vnp_OrderInfo
        public string? ResponseCode { get; private set; }         // vnp_ResponseCode

        protected Payment() { }

        public Payment(int bookingId, decimal amount, string method)
        {
            BookingId = bookingId;
            Amount = amount;
            PaymentMethod = method;
            Status = PaymentStatus.Pending; // Mới tạo luôn là chờ thanh toán
            PaymentDate = DateTime.Now;
        }

        public void SetOrderInfo(string orderInfo)
        {
            OrderInfo = orderInfo;
        }

        // Hàm nghiệp vụ: Xác nhận thanh toán thành công
        public void ConfirmSuccess(string transactionId, string responseCode)
        {
            ExternalTransactionId = transactionId;
            ResponseCode = responseCode;
            Status = PaymentStatus.Success;
            PaymentDate = DateTime.Now;
        }

        // Hàm nghiệp vụ: Thất bại
        public void MarkAsFailed(string responseCode)
        {
            ResponseCode = responseCode;
            Status = PaymentStatus.Failed;
        }
    }

}