using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Llaveremos.SharedLibrary.Middleware
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalException> _logger;

        public GlobalException(RequestDelegate next, ILogger<GlobalException> logger)
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
                _logger.LogError(ex, "Ocurrió una excepción no controlada");
                await ModifyHeader(context, "Error interno", ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string tittle, string message, int statusCode)
        {
            if (context.Response.HasStarted)
            {
                // Ya se empezó a enviar la respuesta, no podemos modificar los headers ni el body
                Console.WriteLine("⚠️ La respuesta ya fue iniciada. No se puede modificar.");
                return;
            }

            context.Response.Clear(); // Borra cualquier contenido anterior
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorObject = new ProblemDetails()
            {
                Title = tittle,
                Detail = message,
                Status = statusCode
            };

            var errorJson = JsonSerializer.Serialize(errorObject);
            await context.Response.WriteAsync(errorJson);
        }
    }
}
