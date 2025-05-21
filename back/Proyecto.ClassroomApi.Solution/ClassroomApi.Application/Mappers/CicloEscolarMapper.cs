using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClassroomApi.Application.Mapper
{
    public static class CicloEscolarMapper
    {
        public static CicloEscolarDTO FromEntity(CicloEscolar entity)
        {
            return new CicloEscolarDTO(
                Id: entity.Id ?? string.Empty, // para evitar null en el DTO
                FechaRegistroCalificaciones: entity.FechaRegistroCalificaciones,
                FechaInicio: entity.FechaInicio,
                FechaFin: entity.FechaFin,
                EsActual: entity.EsActual
            );
        }

        public static CicloEscolar ToEntity(CicloEscolarDTO dto)
        {
            return new CicloEscolar
            {
                Id = dto.Id,
                FechaRegistroCalificaciones = dto.FechaRegistroCalificaciones,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                EsActual = dto.EsActual
            };
        }

        public static List<CicloEscolarDTO> FromEntityList(List<CicloEscolar> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<CicloEscolar> ToEntityList(List<CicloEscolarDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
