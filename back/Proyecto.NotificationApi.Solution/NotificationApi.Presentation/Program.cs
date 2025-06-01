using NotificationApi.Infrastructure.DependencyInjection;
using NotificationApi.Presentation;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
