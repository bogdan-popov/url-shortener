using System.Net;
using System.Text.Json;
using UrlShortener.Exceptions;

namespace UrlShortener.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла необработанная ошибка: {Message}", ex.Message);

                context.Response.ContentType = "application/json";

                HttpStatusCode statusCode;
                string message;

                switch (ex)
                {
                    case BadRequestException badRequestException:
                        statusCode = HttpStatusCode.BadRequest;
                        message = badRequestException.Message;
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        message = "Произошла внутренняя ошибка сервера.";
                        break;
                }

                context.Response.StatusCode = (int)statusCode;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = message,
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
