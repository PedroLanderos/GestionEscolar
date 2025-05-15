using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    public class Calificacion
    {

        [Key]
        public int Id { get; set; }
        public int IdMateria { get; set; }
        public decimal CalificacionFinal { get; set; }
        public string? Comentarios { get; set; }  
        public DateTime FechaRegistro { get; set; }
    }
}
