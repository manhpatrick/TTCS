using HotelManager.Application.DTO.Payments;
using HotelManager.Domain.Entity.Payments;

namespace HotelManager.Application.Converters
{
    public class PaymentConverter
    {
        public PaymentHistoryResponse entityToDto(Payment payment)
        {
            return new PaymentHistoryResponse
            {
                Id = payment.Id,
                Amount = payment.Amount,
                CreatedAt = payment.PaymentDate,
                OrderCode = payment.ExternalTransactionId ?? payment.Id.ToString(),
                Status = (int)payment.Status,
                OrderInfo = payment.OrderInfo!
            };
        }
        public PaymentHistoryAdminResponse entityToDtoAdmin(Payment payment)
        {
            return new PaymentHistoryAdminResponse
            {
                Id = payment.Id,
                Amount = payment.Amount,
                CreatedAt = payment.PaymentDate,
                OrderCode = payment.ExternalTransactionId ?? payment.Id.ToString(),
                Status = (int)payment.Status,
                OrderInfo = payment.OrderInfo!,
                BookingId = payment.BookingId,
                PaymentMethod = payment.PaymentMethod,
                CustomerName = payment.Booking?.Account?.User?.Name,
                Email = payment.Booking?.Account?.User?.Email,
                Phone = payment.Booking?.Account?.User?.Phone
            };
        }
    }
}
