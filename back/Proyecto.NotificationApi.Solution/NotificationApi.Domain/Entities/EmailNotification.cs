using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApi.Domain.Entities
{
    public class EmailNotification
    {
        public string? Destinatario { get; set; } 
        public string? Asunto { get; set; } 
        public string? CuerpoHtml { get; set; }
    }
}
