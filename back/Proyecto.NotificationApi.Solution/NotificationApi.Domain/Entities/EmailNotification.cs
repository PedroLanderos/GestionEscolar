using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApi.Domain.Entities
{
    public class EmailNotification
    {
        public string Para { get; set; } = null!;
        public string Asunto { get; set; } = null!;
        public string MensajeHtml { get; set; } = null!;
        public string? Tipo { get; set; }
    }
}
