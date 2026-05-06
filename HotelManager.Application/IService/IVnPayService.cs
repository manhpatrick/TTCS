using Microsoft.AspNetCore.Http;
using HotelManager.Application.DTO.Payments; // Hoặc thư mục chứa PaymentResponseModel

namespace HotelManager.Application.IService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, PaymentInformationRequest model);
        PaymentResponse PaymentExecute(IQueryCollection collections);
    }
}