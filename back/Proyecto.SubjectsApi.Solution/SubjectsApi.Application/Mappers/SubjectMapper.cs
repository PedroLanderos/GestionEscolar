using SubjectsApi.Application.DTOs;
using SubjectsApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SubjectsApi.Application.Mappers
{
    public static class SubjectMapper
    {
        public static SubjectDTO FromEntity(Subject entity)
        {
            return new SubjectDTO
            {
                Id = entity.Id,
                Nombre = entity.Nombre!,
                Codigo = entity.Codigo!,
                Tipo = entity.Tipo!,
                Grado = entity.Grado,
                EsObligatoria = entity.EsObligatoria,
                EstaActiva = entity.EstaActiva,
                FechaCreacion = entity.FechaCreacion,
                FechaActualizacion = entity.FechaActualizacion
            };
        }

        public static Subject ToEntity(SubjectDTO dto)
        {
            return new Subject
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Codigo = dto.Codigo,
                Tipo = dto.Tipo,
                Grado = dto.Grado,
                EsObligatoria = dto.EsObligatoria,
                EstaActiva = dto.EstaActiva,
                FechaCreacion = dto.FechaCreacion,
                FechaActualizacion = dto.FechaActualizacion
            };
        }

        public static List<SubjectDTO> FromEntityList(List<Subject> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<Subject> ToEntityList(List<SubjectDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
