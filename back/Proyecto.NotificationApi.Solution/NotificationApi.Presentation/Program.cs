using NotificationApi.Infrastructure.DependencyInjection;
using NotificationApi.Presentation;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Registra servicios necesarios, incluyendo el servicio de RabbitMQ y el worker
builder.Services.AddApplicationService(builder.Configuration);

// Agrega el servicio de Worker como un servicio hospedado
builder.Services.AddHostedService<Worker>();

// Construye y ejecuta el host
var host = builder.Build();

// Asegura que el host se ejecute correctamente
await host.RunAsync();
