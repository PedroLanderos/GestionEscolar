using ClassroomApi.Application.Interfaces;
using ClassroomApi.Domain.Entities;
using ClassroomApi.Infrastructure.Data;
using ClassroomApi.Infrastructure.Repositories;
using Llaveremos.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClassroomApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<ClassroomDbContext>(services, config, config["MySerilog:FileName"]!);

           
            services.AddScoped<IClassroom<Asistencia>, ClassroomRepository<Asistencia>>();
            services.AddScoped<IClassroom<Calificacion>, ClassroomRepository<Calificacion>>();
            services.AddScoped<IClassroom<Sancion>, ClassroomRepository<Sancion>>();
            services.AddScoped<IClassroom<Reporte>, ClassroomRepository<Reporte>>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
