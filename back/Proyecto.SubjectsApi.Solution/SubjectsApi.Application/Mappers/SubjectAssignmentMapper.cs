using SubjectsApi.Application.DTOs;
using SubjectsApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SubjectsApi.Application.Mappers
{
    public static class SubjectAssignmentMapper
    {
        public static SubjectAssignmentDTO FromEntity(SubjectAssignment entity)
        {
            return new SubjectAssignmentDTO
            {
                Id = entity.Id,
                SubjectId = entity.SubjectId,
                UserId = entity.UserId
            };
        }

        public static SubjectAssignment ToEntity(SubjectAssignmentDTO dto)
        {
            return new SubjectAssignment
            {
                Id = dto.Id,
                SubjectId = dto.SubjectId,
                UserId = dto.UserId,
                FechaCreacion = DateTime.UtcNow, 
                FechaActualizacion = null
            };
        }

        public static List<SubjectAssignmentDTO> FromEntityList(List<SubjectAssignment> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<SubjectAssignment> ToEntityList(List<SubjectAssignmentDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
