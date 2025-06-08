using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.DTOs
{
    public class EmailNotificationDTO
    {
        public string? Destinatario { get; set; }
        public string? Asunto { get; set; }
        public string? CuerpoHtml { get; set; }

        public EmailNotificationDTO(string destinatario, string asunto, string cuerpoHtml)
        {
            Destinatario = destinatario; Asunto = asunto; CuerpoHtml = cuerpoHtml;
        }
    }
}
