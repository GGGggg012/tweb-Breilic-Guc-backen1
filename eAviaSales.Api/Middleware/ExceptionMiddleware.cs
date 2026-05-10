using System.Net;
using System.Text.Json;
using eAviaSales.Domain.Models;

namespace eAviaSales.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                await WriteResponseAsync(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await WriteResponseAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                await WriteResponseAsync(context, HttpStatusCode.InternalServerError, "Something went wrong");
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode code, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var response = new ActionResponse { IsSuccess = false, Message = message, StatusCode = (int)code };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
