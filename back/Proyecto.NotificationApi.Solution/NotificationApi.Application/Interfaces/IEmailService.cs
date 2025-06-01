using NotificationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApi.Application.Interfaces
{
    public interface IEmailService
    {
        Task EnviarAsync(EmailNotification notificacion);
    }
}
