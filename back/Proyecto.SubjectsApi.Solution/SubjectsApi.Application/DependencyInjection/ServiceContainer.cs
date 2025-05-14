using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubjectsApi.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IUser, User>(client =>
            {
                client.BaseAddress = new Uri("http://authenticationapiservice:5000/api/usuario/");

            });

            return services;
        }
    }
}
