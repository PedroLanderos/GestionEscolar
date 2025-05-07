using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    public class Asistencia
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public bool Asistio { get; set; }
        public string? Justificacion { get; set; }  
    }
}
