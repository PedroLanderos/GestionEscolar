using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AuthenticationApi.Application.Services
{
    public class Email(IConfiguration config) : IEmail
    {
        public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sistema Escolar", config["Email:From"]));
            message.To.Add(MailboxAddress.Parse(destinatario));
            message.Subject = asunto;

            var bodyBuilder = new BodyBuilder { HtmlBody = cuerpoHtml };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // Ignorar errores de certificado (solo para desarrollo)
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await client.ConnectAsync(config["Email:Smtp"], int.Parse(config["Email:Port"]!), SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(config["Email:User"], config["Email:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return true;
        }
    }
}
