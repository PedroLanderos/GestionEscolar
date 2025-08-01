﻿using ClassroomApi.Application.DTOs;
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
                IdAlumno: entity.IdAlumno!,
                Grupo: entity.Grupo,
                CicloEscolar: entity.CicloEscolar,
                Tipo: entity.Tipo!
            );
        }

        public static Reporte ToEntity(ReporteDTO dto)
        {
            return new Reporte
            {
                Id = dto.Id,
                Fecha = dto.Fecha,
                IdAlumno = dto.IdAlumno,
                Grupo = dto.Grupo,
                CicloEscolar = dto.CicloEscolar,
                Tipo = dto.Tipo
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
