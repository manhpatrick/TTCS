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
    }
}
