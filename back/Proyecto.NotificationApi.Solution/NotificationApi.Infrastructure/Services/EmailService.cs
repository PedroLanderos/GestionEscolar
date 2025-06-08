using Llaveremos.SharedLibrary.Logs;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NotificationApi.Application.Interfaces;
using NotificationApi.Domain.Entities;
using System;
using System.Net;

namespace NotificationApi.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration _config)
        {
            config = _config;
        }

        public async Task EnviarAsync(EmailNotification notificacion)
        {
            try
            {
                LogException.LogExceptions(new Exception($"El mensaje es: {notificacion.CuerpoHtml}"));
                var message = new MimeMessage();

                //definir cabecera del mensaje
                message.From.Add(new MailboxAddress("Sistema Escolar", config["Email:From"]));

                //definir destinatario
                message.To.Add(MailboxAddress.Parse(notificacion.Destinatario));

                //definir asunto
                message.Subject = notificacion.Asunto;

                //definir cuerpo del mensaje
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = notificacion.CuerpoHtml
                };
                message.Body = bodyBuilder.ToMessageBody();


                using var client = new SmtpClient();

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(config["Email:Smtp"], int.Parse(config["Email:Port"]!), SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(config["Email:User"], config["Email:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al enviar el correo desde EmailService en notification api", ex);
            }
        }
    }
}
