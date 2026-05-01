
using HotelManager.Domain.Entity.ServiceUsages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Persistence.Configurations
{
    public class ServiceUsageConfiguration : IEntityTypeConfiguration<ServiceUsage>
    {
        public void Configure(EntityTypeBuilder<ServiceUsage> builder)
        {
            builder.ToTable("ServiceUsage"); // Tên bảng trong SQL
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");

            // Quan hệ với Booking
            builder.HasOne(x => x.Booking)
                .WithMany(b => b.ServiceUsages) // Nhớ sửa tên List trong Booking.cs thành ServiceUsages
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Booking -> Xóa luôn danh sách dịch vụ đi kèm

            // Quan hệ với Service (Menu)
            builder.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // Xóa món trong Menu -> Không được xóa lịch sử dùng của khách
        }
    }
}