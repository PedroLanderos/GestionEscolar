using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScheduleApi.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // HttpClient para IArticles apuntando al API Gateway
            services.AddHttpClient<IAuthentication, Authentication>(client =>
            {
                client.BaseAddress = new Uri("http://apigateway:5000/api/article/");
            });

            return services;
        }
    }
}
