using Microsoft.AspNetCore.Http;
using HotelManager.Application.IService;
using HotelManager.Application.IRepository;
using HotelManager.Domain.Entity.Payments;
using HotelManager.Application.DTO.Payments;
using HotelManager.Application.CustomException;

namespace HotelManager.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVnPayService _vnPayService;

        public PaymentService(
            IBookingRepository bookingRepository,
            IPaymentRepository paymentRepository,
            IVnPayService vnPayService)
        {
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _vnPayService = vnPayService;
        }

        public async Task<string> CreateVnPayUrlAsync(int bookingId, HttpContext context)
        {
            // 1. Lấy đơn phòng
            var booking = await _bookingRepository.GetById(bookingId);
            if (booking == null) throw new NotExistsException("Không tìm thấy đơn đặt phòng.");

            // 2. Tạo giao dịch Payment mới (Trạng thái Pending)
            var payment = new Payment(booking.Id, booking.TotalPrice, "VNPAY");
            await _paymentRepository.Add(payment);
            await _paymentRepository.SaveAsync(); // Lưu để sinh ra ID

            // 3. Chuẩn bị cục dữ liệu cho VNPAY
            var paymentInfo = new PaymentInformationRequest
            {
                Amount = booking.TotalPrice,
                OrderDescription = $"Thanh toan cho luot dat phong {booking.Room?.Name} tu {booking.StartTime} den {booking.EndTime}",
                OrderType = "other",
                Name = "Khach Hang",
                TxnRef = payment.Id.ToString() // Mã tham chiếu quan trọng
            };

            // 4. Nhờ VnPayService sinh ra Link
            return _vnPayService.CreatePaymentUrl(context, paymentInfo);
        }

        public async Task<PaymentResponse> ProcessVnPayCallbackAsync(IQueryCollection collections)
        {
            // 1. Nhờ VnPayService dịch kết quả
            var response = _vnPayService.PaymentExecute(collections);

            if (response != null && !string.IsNullOrEmpty(response.OrderId))
            {
                // 2. Tìm lại giao dịch Payment trong DB
                var paymentId = int.Parse(response.OrderId);
                var payment = await _paymentRepository.GetById(paymentId);

                if (payment != null)
                {
                    // 3. Cập nhật trạng thái
                    if (response.Success)
                    {
                        // Hàm ConfirmSuccess đã viết ở file Entity Payment.cs
                        payment.ConfirmSuccess(response.TransactionId, response.VnPayResponseCode);

                        var booking = await _bookingRepository.GetById(payment.BookingId);
                        booking.MarkAsPaid(); // Hàm MarkAsPaid đã viết ở file Entity Booking.cs
                        await _bookingRepository.Update(booking.Id, booking);
                    }
                    else
                    {
                        payment.MarkAsFailed(response.VnPayResponseCode);
                    }
                    // Giả sử collections là dữ liệu VNPAY trả về
                    var vnp_OrderInfo = collections["vnp_OrderInfo"];

                    // Gán vào thực thể payment trước khi update
                    payment.SetOrderInfo(vnp_OrderInfo);

                    // 4. Lưu CSDL
                    await _paymentRepository.Update(payment.Id, payment);
                }
            }
            return response;
        }
    }
}