using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClassroomApi.Application.Mapper
{
    public static class CalificacionMapper
    {
        public static CalificacionDTO FromEntity(Calificacion entity)
        {
            return new CalificacionDTO(
                Id: entity.Id,
                IdMateria: entity.IdMateria ?? string.Empty,  
                IdAlumno: entity.IdAlumno,
                CalificacionFinal: entity.CalificacionFinal,
                Comentarios: entity.Comentarios,
                IdCiclo: entity.IdCiclo ?? string.Empty      
            );
        }

        public static Calificacion ToEntity(CalificacionDTO dto)
        {
            return new Calificacion
            {
                Id = dto.Id,
                IdMateria = dto.IdMateria,
                IdAlumno = dto.IdAlumno,
                CalificacionFinal = dto.CalificacionFinal,
                Comentarios = dto.Comentarios,
                IdCiclo = dto.IdCiclo
            };
        }

        public static List<CalificacionDTO> FromEntityList(List<Calificacion> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<Calificacion> ToEntityList(List<CalificacionDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
