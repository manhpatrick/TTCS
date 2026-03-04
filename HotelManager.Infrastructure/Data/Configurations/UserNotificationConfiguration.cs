using HotelManager.Domain.Entity.UserNotifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Infrastructure.Data.Configurations
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotification");
            builder.HasKey(un => un.Id);
            builder.Property(un => un.Id).ValueGeneratedOnAdd();
            builder.HasOne(un => un.Account).WithMany().HasForeignKey(un => un.AccountId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(un => un.Notification).WithMany(n => n.UserNotifications).HasForeignKey(un => un.NotificationId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
