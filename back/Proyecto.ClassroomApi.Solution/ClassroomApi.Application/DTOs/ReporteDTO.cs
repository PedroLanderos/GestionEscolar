using System;
using System.ComponentModel.DataAnnotations;

namespace ClassroomApi.Application.DTOs
{
    public record ReporteDTO
    (
        int Id,

        [Required]
        DateTime Fecha,

        [Required]
        string IdAlumno,

        string? Grupo,

        string? CicloEscolar,

        [Required]
        string Tipo 
    );
}
