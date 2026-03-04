using HotelManager.Domain.Entity.Accounts;  
using HotelManager.Domain.Entity.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Username).IsRequired().HasMaxLength(256).IsUnicode(false);
            builder.HasIndex(x => x.Username).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Role).IsRequired().HasConversion<string>().HasMaxLength(256);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.HasOne(a => a.User).WithOne(u => u.Account).HasForeignKey<User>(u => u.AccountId).
                OnDelete(DeleteBehavior.Cascade);
        }
    }
}
