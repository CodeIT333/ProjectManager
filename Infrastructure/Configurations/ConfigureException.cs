using Domain.Commons.Models;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Configurations
{
    // global exception handling middleware
    public class ConfigureException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ConfigureException> _logger;

        public ConfigureException(RequestDelegate next, ILogger<ConfigureException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = exception switch
            {
                BadRequestException => new ErrorResponse { StatusCode = 400, Message = exception.Message, ExceptionType = "BadRequestException" },
                NotFoundException => new ErrorResponse { StatusCode = 404, Message = exception.Message, ExceptionType = "NotFoundException" },
                _ => new ErrorResponse { StatusCode = 500, Message = "An unexpected error occurred.", ExceptionType = "InternalServerError" } // default
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
