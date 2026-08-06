using CarServiceApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace CarServiceApi.Middleware
{
    /// <summary>
    /// Central place to translate the small set of expected business exceptions
    /// (not found / unauthorized / forbidden / invalid operation) into the right
    /// HTTP status codes, instead of every controller action repeating its own
    /// try/catch. Anything unexpected is logged and returned as a generic 500
    /// so internal details never leak to the client.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                var (statusCode, message) = MapException(ex);

                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    _logger.LogError(ex, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var payload = JsonSerializer.Serialize(new { message });
                await context.Response.WriteAsync(payload);
            }
        }

        private static (HttpStatusCode StatusCode, string Message) MapException(Exception ex) => ex switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),
            ForbiddenAccessException => (HttpStatusCode.Forbidden, ex.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
        };
    }
}
