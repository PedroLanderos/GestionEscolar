using Llaveremos.SharedLibrary.Logs;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NotificationApi.Application.Interfaces;
using NotificationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApi.Infrastructure.Services
{
    public class EmailService(IConfiguration _config) : IEmailService
    {
        public async Task EnviarAsync(EmailNotification notificacion)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sistema Escolar", _config["Email:From"]));
                message.To.Add(MailboxAddress.Parse(notificacion.Para));
                message.Subject = notificacion.Asunto;

                var bodyBuilder = new BodyBuilder { HtmlBody = notificacion.MensajeHtml };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // Ignora errores de certificado (solo en desarrollo, no recomendado en producción)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(
                    _config["Email:Smtp"],
                    int.Parse(_config["Email:Port"]!),
                    SecureSocketOptions.SslOnConnect);

                await client.AuthenticateAsync(
                    _config["Email:User"],
                    _config["Email:Password"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); // Usa tu logger centralizado
                throw new Exception("Error al enviar el correo desde NotificationApi");
            }
        }
    }
}
