using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.DTOs
{
    public record CalificacionDTO(
       int Id,
       [Required] int IdMateria, 
       [Required] decimal CalificacionFinal,  
       string Comentarios,  
       [Required] DateTime FechaRegistro  
   );
}
