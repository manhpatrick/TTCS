
using HotelManager.Domain.Entity.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.BirthDay).HasColumnType("date");

            builder.HasOne(u => u.Account).WithOne(a => a.User).HasForeignKey<User>(u => u.AccountId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
