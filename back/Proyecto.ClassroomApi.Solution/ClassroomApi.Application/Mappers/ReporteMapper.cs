using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassroomApi.Application.Mapper
{
    public static class ReporteMapper
    {
        public static ReporteDTO FromEntity(Reporte entity)
        {
            return new ReporteDTO(
                Id: entity.Id,
                Fecha: entity.Fecha,
                Descripcion: entity.Descripcion!,
                TipoReporte: entity.TipoReporte!
            );
        }

        public static Reporte ToEntity(ReporteDTO dto)
        {
            return new Reporte
            {
                Id = dto.Id,
                Fecha = dto.Fecha,
                Descripcion = dto.Descripcion,
                TipoReporte = dto.TipoReporte
            };
        }

        public static List<ReporteDTO> FromEntityList(List<Reporte> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<Reporte> ToEntityList(List<ReporteDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
