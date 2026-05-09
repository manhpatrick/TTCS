using HotelManager.Application.DTO.AppConfig;
using HotelManager.Application.DTO;
using HotelManager.Infrastructure.Data;
using HotelManager.Presentation.Middleware;
using HotelManager.Presentation.Hubs;
using Microsoft.EntityFrameworkCore;
using HotelManager.Infrastructure.AddLayer;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HotelManager.Application.AddLayer;
using HotelManager.Infrastructure.Services;

namespace HotelManager.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                    .AddJsonOptions(opt =>
                    {
                        opt.JsonSerializerOptions.Converters.Add(
                            new JsonStringEnumConverter());
                    });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<AppDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            var JwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = JwtSettings["Key"];
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JwtSettings["Issuer"],
                        ValidAudience = JwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                    option.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers["Token-Expired"] = "true";
                            }
                            return Task.CompletedTask;
                        }
                    };
                }
            );
            // Đăng ký MailSettings
             builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            builder.Services.AddMemoryCache();
            builder.Services.AddSignalR();
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllowReactApp",
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // Quan trọng cho SignalR
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseCors("AllowReactApp");

            app.UseMiddleware<ExceptionMiddleware>();

            // app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            
            // Ánh xạ SignalR Hub
            app.MapHub<NotificationHub>("/hubs/notification");

            app.Run();
        }
    }
}
