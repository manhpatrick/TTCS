using HotelManager.Domain.Entity.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.NumOfPeople).IsRequired();
            builder.Property(b => b.StartTime).IsRequired();
            builder.Property(b => b.EndTime).IsRequired();
            builder.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.TotalPrice).IsRequired().HasColumnType("decimal(18,2)"); ;
            builder.Property(b => b.RoomPriceAtBooking).IsRequired().HasColumnType("decimal(18,2)"); 
            builder.Property(b => b.BookingStatus).IsRequired().HasConversion<string>().HasMaxLength(256);
            builder.HasIndex(b => new { b.RoomId, b.StartTime, b.EndTime }).IsUnique();
            builder.HasOne(b => b.Account).WithMany().HasForeignKey(b => b.AccountId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.Room).WithMany().HasForeignKey(b => b.RoomId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
