using System.Net;
using System.Text.Json;

namespace Interrapidisimo.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Continuar con el pipeline de la solicitud
                await _next(context);
            }
            catch (Exception ex)
            {
                // se captura cualquier excepción no manejada
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
                _ => (int)HttpStatusCode.InternalServerError // 500
            };

            response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new { message = exception?.Message });
            return response.WriteAsync(result);
        }
    }
}
