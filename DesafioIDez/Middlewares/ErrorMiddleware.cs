using DesafioIDez.Api.DTOs;
using System.Net;
using System.Text.Json;

namespace DesafioIDez.Api.Middlewares
{
    public class ErrorMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = ex switch
                {
                    ApplicationException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };
                ErroDTO errorResponse = new(HttpStatusCode.InternalServerError.ToString(), ex.InnerException?.ToString(), ex.Message, ex.StackTrace, ex.Source);
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}
