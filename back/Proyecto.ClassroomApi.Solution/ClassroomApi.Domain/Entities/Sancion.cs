using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    public class Sancion
    {
        [Key]
        public int Id { get; set; }
        public string? TipoSancion { get; set; }  
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string? IdProfesor { get; set; }  
        public string? IdAlumno { get; set; }   
    }
}
