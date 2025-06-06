using AuthenticationApi.Application.DTOs;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Services
{
    public class NotificationPublisher : INotificationPublisher
    {
        private readonly IConfiguration _config;

        public NotificationPublisher(IConfiguration config)
        {
            _config = config;
        }

        public void PublishEmailNotification(EmailNotificationDTO notification)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMq:Host"],
                UserName = _config["RabbitMq:User"] ?? "guest",
                Password = _config["RabbitMq:Password"] ?? "guest"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _config["RabbitMq:Queue"], durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(notification));

            channel.BasicPublish(exchange: "", routingKey: _config["RabbitMq:Queue"], basicProperties: null, body: body);
        }
    }
}
