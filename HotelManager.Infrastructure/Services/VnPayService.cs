using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using HotelManager.Application.IService;
using HotelManager.Infrastructure.Libraries;
using HotelManager.Application.DTO.Payments;

namespace HotelManager.Infrastructure.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(HttpContext context, PaymentInformationRequest model)
        {
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            vnpay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            // Sử dụng kiểu long để loại bỏ hoàn toàn phần thập phân và dấu phẩy
            vnpay.AddRequestData("vnp_Amount", ((long)(model.Amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            // Utils.GetIpAddress() bạn lấy từ file hướng dẫn của VNPAY
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", model.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", model.OrderType);
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:PaymentBackReturnUrl"]);

            // Lấy ID của Payment truyền vào làm mã giao dịch tham chiếu
            vnpay.AddRequestData("vnp_TxnRef", model.TxnRef);

            return vnpay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["Vnpay:HashSecret"]);
            if (!checkSignature) return new PaymentResponse { Success = false };

            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");

            return new PaymentResponse
            {
                Success = vnp_ResponseCode == "00",
                PaymentMethod = "VnPay",
                OrderDescription = vnpay.GetResponseData("vnp_OrderInfo"),
                OrderId = vnpay.GetResponseData("vnp_TxnRef"), // Chính là Payment.Id
                TransactionId = vnpay.GetResponseData("vnp_TransactionNo"),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}