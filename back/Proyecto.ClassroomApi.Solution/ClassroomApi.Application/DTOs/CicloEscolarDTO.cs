using System;
using System.ComponentModel.DataAnnotations;

namespace ClassroomApi.Application.DTOs
{
    public record CicloEscolarDTO(
        [Required]
        string Id,  // El Id es string y no nullable, por eso requerido

        [Required]
        DateTime FechaRegistroCalificaciones,

        [Required]
        DateTime FechaInicio,

        [Required]
        DateTime FechaFin,

        [Required]
        bool EsActual
    );
}
