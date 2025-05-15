using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    public class Asistencia
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public bool Asistio { get; set; }
        public string? Justificacion { get; set; }  
    }
}
