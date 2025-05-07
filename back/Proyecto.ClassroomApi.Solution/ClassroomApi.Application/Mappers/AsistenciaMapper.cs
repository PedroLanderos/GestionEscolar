using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassroomApi.Application.Mapper
{
    public static class AsistenciaMapper
    {
        public static AsistenciaDTO FromEntity(Asistencia entity)
        {
            return new AsistenciaDTO(
                Id: entity.Id,
                Fecha: entity.Fecha,
                Asistio: entity.Asistio,
                Justificacion: entity.Justificacion!
            );
        }

        public static Asistencia ToEntity(AsistenciaDTO dto)
        {
            return new Asistencia
            {
                Id = dto.Id,
                Fecha = dto.Fecha,
                Asistio = dto.Asistio,
                Justificacion = dto.Justificacion
            };
        }

        public static List<AsistenciaDTO> FromEntityList(List<Asistencia> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<Asistencia> ToEntityList(List<AsistenciaDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
