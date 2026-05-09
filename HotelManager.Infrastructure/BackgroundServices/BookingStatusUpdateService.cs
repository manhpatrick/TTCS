using HotelManager.Domain.Entity.Bookings.Enum;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HotelManager.Infrastructure.Services
{
    public class BookingStatusUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookingStatusUpdateService> _logger;

        // Cập nhật: Cho chạy 1 phút 1 lần thay vì 1 tiếng để bắt chính xác mốc 10 phút
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        public BookingStatusUpdateService(IServiceProvider serviceProvider, ILogger<BookingStatusUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Booking Status Update Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateExpiredBookingsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi xảy ra khi tự động cập nhật trạng thái Booking.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task UpdateExpiredBookingsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var now = DateTime.UtcNow;

            // =========================================================
            // 1. TÍNH NĂNG MỚI: Huỷ Booking Pending nếu quá 10 phút
            // =========================================================
            var timeoutLimit = now.AddMinutes(-10);
            var timeoutBookings = await dbContext.Bookings
                .Where(b => b.BookingStatus == BookingStatus.Pending && b.CreatedAt <= timeoutLimit)
                .ToListAsync();

            if (timeoutBookings.Any())
            {
                foreach (var booking in timeoutBookings)
                {
                    // Chuyển status booking thành Cancelled
                    booking.ChangeBookingStatus(BookingStatus.Cancelled);
                }
                _logger.LogInformation($"Đã tự động huỷ {timeoutBookings.Count} bookings do không thanh toán trong 10 phút.");
            }

            // =========================================================
            // 2. TÍNH NĂNG CŨ: Cập nhật booking đã qua ngày CheckOut
            // =========================================================
            var expiredBookings = await dbContext.Bookings
                .Where(b => b.EndTime < now &&
                           (b.BookingStatus == BookingStatus.CheckedIn || b.BookingStatus == BookingStatus.Confirmed))
                .ToListAsync();

            if (expiredBookings.Any())
            {
                foreach (var booking in expiredBookings)
                {
                    if (booking.BookingStatus == BookingStatus.CheckedIn)
                    {
                        booking.ChangeBookingStatus(BookingStatus.Completed);
                    }
                    else if (booking.BookingStatus == BookingStatus.Confirmed)
                    {
                        booking.ChangeBookingStatus(BookingStatus.Cancelled);
                    }
                }
                _logger.LogInformation($"Đã tự động cập nhật trạng thái cho {expiredBookings.Count} bookings bị quá hạn lưu trú.");
            }

            // Lưu tất cả thay đổi xuống Database 1 lần
            if (timeoutBookings.Any() || expiredBookings.Any())
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}