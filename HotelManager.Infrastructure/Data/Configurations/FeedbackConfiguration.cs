using HotelManager.Domain.Entity.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedback");
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();
            builder.Property(f => f.Title).IsRequired().HasMaxLength(1000);
            builder.Property(f => f.Content).IsRequired();
            builder.Property(f => f.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(f => f.IsRead).IsRequired().HasDefaultValue(false);
            builder.HasOne(f => f.Account).WithMany().HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
