using HotelManager.Application.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManager.Application.AddLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<RoomConverter>();
            services.AddScoped<UserConverter>();
            services.AddScoped<ServiceConverter>();
            services.AddScoped<BookingConverter>();
            services.AddScoped<NotificationConverter>();
            services.AddScoped<RatingConverter>();
            services.AddScoped<FeedbackConverter>();
            return services;
        }
    }


}