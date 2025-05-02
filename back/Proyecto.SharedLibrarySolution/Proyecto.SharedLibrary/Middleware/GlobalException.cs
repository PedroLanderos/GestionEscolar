using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Llaveremos.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Llaveremos.SharedLibrary.Middleware
{
    //maneja las excepciones desde el middleware y personaliza las respuestas http dependiendo el codigo
    public class GlobalException(RequestDelegate next) //delegado que acepta un http context y devuelve un task (operacion asincrona) y lo manda al pipeline para manejar la operacion
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //mensajes por deafult
            string message = "Interal error server";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string tittle = "Error";

            try
            {
                await next(context);
                
                //se manda a llamar cuando se hacen muchas request en poco tiempo y se tiene un codigo 429.
                if(context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    tittle = "Warning";
                    message = "Too many messages made";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, tittle, message, statusCode);
                }

                //si la response dice que el cliente no esta autorizado
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    tittle = "Alert";
                    message = "Client with no authorization";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, tittle, message, statusCode);
                }

                //estatus 403: request prohibida
                if(context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    tittle = "No access";
                    message = "You don't have access to this";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, tittle, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    tittle = "out of time";
                    message = "Request time out";
                    statusCode = (int)StatusCodes.Status408RequestTimeout;
                }
                await ModifyHeader(context, tittle, message, statusCode);
            }
        }

        //metodo generico que se llama cada vez que se quieren especificar los parametros de mensaje titulo y estatus en alguno de los casos de mala llamada o demasiadas request

        private static async Task ModifyHeader(HttpContext context, string tittle, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = tittle
            }), CancellationToken.None);
            return;
        }
    }
}
