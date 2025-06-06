using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs
{
    public class EmailNotificationDTO
    {
        public string Para { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }

        public EmailNotificationDTO(string para, string asunto, string contenido)
        {
            Para = para;
            Asunto = asunto;
            Contenido = contenido;
        }
    }
}
