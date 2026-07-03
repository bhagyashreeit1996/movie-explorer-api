using System.Net;
using System.Text.Json;
using MovieExplorer.API.Core.Exceptions;

namespace MovieExplorer.API.Infrastructure.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;

            switch (exception)
            {
                case MovieNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case MovieAlreadyLikedException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;

                case ArgumentException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = exception.Message,
                statusCode = response.StatusCode
            });

            await response.WriteAsync(result);
        }
    }
}