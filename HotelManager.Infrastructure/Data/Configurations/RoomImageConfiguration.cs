using HotelManager.Domain.Entity.RoomImages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class RoomImageConfiguration : IEntityTypeConfiguration<RoomImage>
    {
        public void Configure(EntityTypeBuilder<RoomImage> builder)
        {
            builder.ToTable("RoomImage");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500); // Link ảnh không nên quá dài

            // Cấu hình quan hệ: 1 Room - Nhiều Image
            builder.HasOne(img => img.Room)
                .WithMany(r => r.RoomImages) // Room trỏ về list RoomImages
                .HasForeignKey(img => img.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
            // 💡 Dùng Cascade: Nếu xóa Phòng -> Xóa sạch ảnh của phòng đó (tránh rác DB).
        }
    }
}