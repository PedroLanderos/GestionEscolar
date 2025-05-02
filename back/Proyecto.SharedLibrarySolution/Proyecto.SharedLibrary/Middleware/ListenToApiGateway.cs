using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Llaveremos.SharedLibrary.Middleware
{
    public class ListenToApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var header = context.Request.Headers["Api-Gateway"]; 
            
            //si el valor es null significa que no viene del api gateway y la solicitud no es valida si no viene de ahi.
            if(header.FirstOrDefault() == null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service is not available");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
