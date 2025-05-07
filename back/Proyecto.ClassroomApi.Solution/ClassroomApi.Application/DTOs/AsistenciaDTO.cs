using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.DTOs
{
    public record AsistenciaDTO(
        int Id,
        [Required] DateTime Fecha,  // Fecha de la clase
        [Required] bool Asistio,  // Indica si el estudiante asistió o no
        string Justificacion  // Justificación de la falta, si la hubiera
    );
}
