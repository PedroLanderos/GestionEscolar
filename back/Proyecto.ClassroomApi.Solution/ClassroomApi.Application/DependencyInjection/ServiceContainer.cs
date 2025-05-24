using ClassroomApi.Application.Services;
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
            services.AddHttpClient<IAuthentication, Authentication>(client =>
            {
                client.BaseAddress = new Uri("http://authenticationapiservice:5000/api/");
            });
            services.AddHttpClient<ISubject, Subject>(client =>
            {
                client.BaseAddress = new Uri("http://subjectsapiservice:5001/api/");
            });
            services.AddHttpClient<ISchedule, Schedule>(client =>
            {
                client.BaseAddress = new Uri("http://scheduleapiservice:5002/api/"); 
            });

            return services;
        }
    }
}