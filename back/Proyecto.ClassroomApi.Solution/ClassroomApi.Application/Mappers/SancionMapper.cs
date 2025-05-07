using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassroomApi.Application.Mapper
{
    public static class SancionMapper
    {
        public static SancionDTO FromEntity(Sancion entity)
        {
            return new SancionDTO(
                Id: entity.Id,
                TipoSancion: entity.TipoSancion!,
                Descripcion: entity.Descripcion!,
                Fecha: entity.Fecha,
                IdProfesor: entity.IdProfesor,
                IdAlumno: entity.IdAlumno
            );
        }

        public static Sancion ToEntity(SancionDTO dto)
        {
            return new Sancion
            {
                Id = dto.Id,
                TipoSancion = dto.TipoSancion,
                Descripcion = dto.Descripcion,
                Fecha = dto.Fecha,
                IdProfesor = dto.IdProfesor,
                IdAlumno = dto.IdAlumno
            };
        }

        public static List<SancionDTO> FromEntityList(List<Sancion> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<Sancion> ToEntityList(List<SancionDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
