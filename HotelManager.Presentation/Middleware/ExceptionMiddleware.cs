using HotelManager.Application.CustomException;
using HotelManager.Application.CustomException.Auth;
using HotelManager.Application.CustomException.Rooms;
using HotelManager.Domain.Exceptions;
using System.Text.Json;

namespace HotelManager.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context,Exception exception)
        {
            var statusCode = exception switch
            {
                UsernameAlreadyExistException => StatusCodes.Status409Conflict,
                PasswordIncorrectException => StatusCodes.Status401Unauthorized,
                PasswordIsShortException => StatusCodes.Status400BadRequest,
                UsernameNotExistException => StatusCodes.Status401Unauthorized,
                DomainException => StatusCodes.Status400BadRequest,
                ForbiddenException => StatusCodes.Status403Forbidden,
                NotExistsException => StatusCodes.Status404NotFound,
                RoomNotAvailableException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            var response = new
            {
                status = statusCode,
                error = exception.Message
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
