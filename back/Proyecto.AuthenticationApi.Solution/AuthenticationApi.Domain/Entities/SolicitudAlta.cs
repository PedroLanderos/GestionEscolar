using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entities
{
    public class SolicitudAlta
    {
        public int Id { get; set; }
        public string NombreAlumno { get; set; } = null!;
        public string CurpAlumno { get; set; } = null!;
        public int Grado { get; set; }
        public string NombrePadre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string CorreoPadre { get; set; } = null!;
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        public bool Procesado { get; set; } = false;
    }
}
