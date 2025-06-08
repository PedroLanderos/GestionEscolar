using Llaveremos.SharedLibrary.Logs;
using NotificationApi.Application.Interfaces;
using NotificationApi.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace NotificationApi.Application.UseCases
{
    public class EnviarEmailHandler
    {
        private readonly IEmailService _emailService;

        public EnviarEmailHandler(IEmailService s)
        {
            _emailService = s;
        }

        public async Task HandleAsync(EmailNotification notificacion)
        {
            try
            {
                await _emailService.EnviarAsync(notificacion);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al enviar el correo desde EmailService", ex);
            }
        }
    }
}
