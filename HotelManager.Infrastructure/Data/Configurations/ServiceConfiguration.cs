using HotelManager.Domain.Entity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Service");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.Name).IsRequired().HasMaxLength(256);
            builder.Property(s => s.Category).IsRequired().HasConversion<string>().HasMaxLength(256);
            builder.Property(s => s.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(s => s.Unit).IsRequired().HasMaxLength(256);
            builder.Property(s => s.IsActive).IsRequired();

        }
    }
}
