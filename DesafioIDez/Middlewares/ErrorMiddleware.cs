using DesafioIDez.Api.DTOs;
using DesafioIDez.Infraestrutura.Excecoes;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesafioIDez.Api.Middlewares
{
    public class ErrorMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

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

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex switch
            {
                AppException appEx => appEx.StatusCode,
                ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var erro = new ErroDTO(
                Status: statusCode.ToString(),
                Mensagem: ex.Message
            );

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(erro, jsonOptions)
            );
        }
    }
}
