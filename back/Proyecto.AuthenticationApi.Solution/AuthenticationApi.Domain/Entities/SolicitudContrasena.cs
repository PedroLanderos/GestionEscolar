using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entities
{
    public class SolicitudContrasena
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        public bool? Procesada { get; set; } = false;
    }
}
