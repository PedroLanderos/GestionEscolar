using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationApi.Application.UseCases;
using NotificationApi.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationApi.Presentation;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private readonly EnviarEmailHandler _handler;
    private IConnection? _connection;
    private IModel? _channel;

    public Worker(ILogger<Worker> logger, IConfiguration config, EnviarEmailHandler handler)
    {
        _logger = logger;
        _config = config;
        _handler = handler;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMq:Host"],
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _config["RabbitMq:Queue"],
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _logger.LogInformation("Conexión establecida con RabbitMQ.");
            return base.StartAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar conexión con RabbitMQ.");
            throw;
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Mensaje recibido: {message}", json);

                var notificacion = JsonSerializer.Deserialize<EmailNotification>(json);

                if (notificacion != null)
                {
                    _logger.LogInformation("Correo a enviar: {email}", notificacion.Destinatario);
                    await _handler.HandleAsync(notificacion);
                    _logger.LogInformation("Correo enviado a {email}", notificacion.Destinatario);
                    _channel!.BasicAck(ea.DeliveryTag, multiple: false);
                }
                else
                {
                    _logger.LogWarning("Mensaje no se pudo deserializar.");
                    _channel!.BasicNack(ea.DeliveryTag, false, true); // Reenviar mensaje a la cola en caso de error
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar mensaje.");
                _channel!.BasicNack(ea.DeliveryTag, false, true); // Reenviar mensaje a la cola en caso de error
            }
        };

        _channel.BasicConsume(
            queue: _config["RabbitMq:Queue"],
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation("Escuchando eventos de notificación...");
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _channel?.Close();
            _connection?.Close();
            _logger.LogInformation("Conexión con RabbitMQ cerrada.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cerrar la conexión con RabbitMQ.");
        }

        return base.StopAsync(cancellationToken);
    }
}
