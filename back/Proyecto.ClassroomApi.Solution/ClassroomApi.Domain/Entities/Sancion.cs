using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    class Sancion
    {
        public int Id { get; set; }
        public string? TipoSancion { get; set; }  
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int IdProfesor { get; set; }  
        public int IdAlumno { get; set; }   
    }
}
