using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotificationApi.Application.Interfaces;
using NotificationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            await _emailService.EnviarAsync(notificacion);
        }
    }
}
