using HotelManager.Domain.Entity.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // 1. Tên bảng trong Database
            builder.ToTable("Payments");

            // 2. Khóa chính
            builder.HasKey(x => x.Id);

            // 3. Cấu hình các cột dữ liệu
            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)") // Đảm bảo độ chính xác cho tiền tệ
                .IsRequired();

            builder.Property(x => x.PaymentMethod)
                .HasMaxLength(50) // Giới hạn độ dài chuỗi "VNPAY", "Tiền mặt"...
                .IsRequired();

            builder.Property(x => x.PaymentDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            // Các trường từ VNPAY có thể để null nếu chưa thanh toán xong
            builder.Property(x => x.ExternalTransactionId)
                .HasMaxLength(100);

            builder.Property(x => x.ResponseCode)
                .HasMaxLength(10);

            builder.Property(x => x.OrderInfo)
                .HasMaxLength(500);

            // 4. Thiết lập quan hệ (Khóa ngoại) với bảng Booking
            // Một Booking có thể có nhiều bản ghi Payment (do khách có thể thử lại nhiều lần)
            builder.HasOne<HotelManager.Domain.Entity.Bookings.Booking>()
                .WithMany()
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Booking thì xóa luôn Payment liên quan
        }
    }
}