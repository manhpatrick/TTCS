using HotelManager.Domain.Entity.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Room");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.Name).IsRequired().HasMaxLength(1000);
            builder.Property(r => r.Description).HasMaxLength(1000);
            builder.Property(r => r.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(256);
            builder.Property(r => r.RoomStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(256);
            builder.Property(r => r.PricePerNight).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(r => r.AverageStar).HasColumnType("decimal(3,1)");
        }
    }
}
