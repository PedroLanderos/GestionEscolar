using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using System;
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
                IdMateria: entity.IdMateria,
                CalificacionFinal: entity.CalificacionFinal,
                Comentarios: entity.Comentarios!,
                FechaRegistro: entity.FechaRegistro
            );
        }

        public static Calificacion ToEntity(CalificacionDTO dto)
        {
            return new Calificacion
            {
                Id = dto.Id,
                IdMateria = dto.IdMateria,
                CalificacionFinal = dto.CalificacionFinal,
                Comentarios = dto.Comentarios,
                FechaRegistro = dto.FechaRegistro
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
