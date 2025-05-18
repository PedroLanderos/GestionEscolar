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
        int IdAlumno,

        string? Grupo,

        string? CicloEscolar,

        string? IdHorario,

        [Required]
        string Tipo 
    );
}
