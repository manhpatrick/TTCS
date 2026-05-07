using Microsoft.AspNetCore.Http;
using HotelManager.Application.DTO.Payments;

namespace HotelManager.Application.IService
{
    public interface IPaymentService
    {
        Task<string> CreateVnPayUrlAsync(int bookingId, HttpContext context);
        Task<PaymentResponse> ProcessVnPayCallbackAsync(IQueryCollection collections);
        Task<IEnumerable<PaymentHistoryResponse>> GetUserPayments(int accountId);
    }
}