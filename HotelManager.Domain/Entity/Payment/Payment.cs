// File: HotelManager.Domain/Entity/Payment/Payment.cs
using HotelManager.Domain.Entity.Payment.Enum;

namespace HotelManager.Domain.Entity.Payment
{
    public class Payment
    {
        public int Id { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public int AccountId { get; private set; }
        public int BookingId { get; private set; }

        // CÁC TRƯỜNG CẦN BỔ SUNG
        public string? TransactionId { get; private set; } // Mã giao dịch từ cổng thanh toán
        public string? PaymentMethod { get; private set; } // Ví dụ: "VNPAY", "MOMO"
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected Payment() { }

        public Payment(decimal amount, int accountId, int bookingId, string method)
        {
            Amount = amount;
            AccountId = accountId;
            BookingId = bookingId;
            PaymentMethod = method;
            Status = PaymentStatus.Pending; // Mặc định là đang chờ
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsSuccess(string transactionId)
        {
            Status = PaymentStatus.Success;
            TransactionId = transactionId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            Status = PaymentStatus.Failed;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}