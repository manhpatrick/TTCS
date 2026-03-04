using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Entity.Ratings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Rating");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.NumOfRating).IsRequired();
            builder.Property(r => r.Review).IsRequired().HasMaxLength(1000);
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.HasOne(r => r.Booking).WithOne(b => b.Rating).HasForeignKey<Rating>(r => r.BookingId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
