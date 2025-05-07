using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    public class Reporte
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public string? TipoReporte { get; set; } 
        public int IdAlumno { get; set; }
    }
}
