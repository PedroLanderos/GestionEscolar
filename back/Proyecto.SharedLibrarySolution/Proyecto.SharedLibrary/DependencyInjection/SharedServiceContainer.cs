using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Llaveremos.SharedLibrary.Middleware;

namespace Llaveremos.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        //metodo que sirve para agregar los servicios creados por nosotros y el middleware de .net 
        //IServiceCollection es en donde se configuaran los servicios de la app (como las bases de datos)
        public static IServiceCollection AddSharedServices<TContext> 
            (this IServiceCollection services, IConfiguration config, string fileName) where TContext : DbContext
        {
            //agregar el contexto de la base de datos que se utilizara (se agrega la referencia a la cadena)
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("llaveremosConnection"), sqlServerOption => sqlServerOption.EnableRetryOnFailure()));

            //configurar los logs de serilog (la libreria de los logs)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-text", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                //define el formato de los logs 
                outputTemplate: "{Timestamp:yyyy--MM--dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day).CreateLogger();
            
            //agregar el scheme del jwt llamando al metodo que se creo en jwtauthenticacionscheme
            JWTAuthenticationScheme.AddJWTSchemeCollection(services, config);

            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
            app.UseMiddleware<ListenToApiGateway>();
            return app;
        }
    }
}
