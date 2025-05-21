using System;
using System.ComponentModel.DataAnnotations;

namespace ClassroomApi.Application.DTOs
{
    public record CalificacionDTO(
        int Id,

        [Required]
        string IdMateria,    

        string? IdAlumno,     

        [Required]
        decimal CalificacionFinal,

        string? Comentarios,

        [Required]
        string IdCiclo        
    );
}
