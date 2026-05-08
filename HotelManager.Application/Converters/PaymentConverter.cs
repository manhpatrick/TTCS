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
    }
}
