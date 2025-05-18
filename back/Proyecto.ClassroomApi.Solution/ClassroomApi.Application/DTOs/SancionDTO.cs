using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.DTOs
{
    public record SancionDTO(
        int Id,
        [Required] string TipoSancion,   
        string Descripcion,  
        [Required] DateTime Fecha,  
        [Required] string IdProfesor,  
        [Required] string IdAlumno   
    );
}
