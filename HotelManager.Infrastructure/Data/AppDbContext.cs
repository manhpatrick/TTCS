using HotelManager.Domain.Entity.Accounts;
using HotelManager.Domain.Entity.Bookings;
using HotelManager.Domain.Entity.Notifications;
using HotelManager.Domain.Entity.Ratings;
using HotelManager.Domain.Entity.Rooms;
using HotelManager.Domain.Entity.Services;
using HotelManager.Domain.Entity.UserNotifications;
using HotelManager.Domain.Entity.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
