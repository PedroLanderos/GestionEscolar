using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? IdAlumno { get; set; }
        public string? Grupo { get; set; }
        public string? CicloEscolar { get; set; }
        public string? Tipo { get; set; } //puede ser inasistencia o mala conducta 
    }

}
