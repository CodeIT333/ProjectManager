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
            var statusCode = exception switch
            {
                // add new exception types here
                BadRequestException => 400,
                NotFoundException => 404,
                _ => 500                    // default
            };

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = exception.Message,
                ExceptionType = exception.GetType().Name
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
