using System;
using System.ComponentModel.DataAnnotations;

namespace ClassroomApi.Application.DTOs
{
    public record AsistenciaDTO(
        int Id,

        [Required]
        DateTime Fecha,

        [Required]
        bool Asistio,

        string? Justificacion,

        [Required]
        string? IdAlumno,

        [Required]
        string? IdProfesor
    );
}
