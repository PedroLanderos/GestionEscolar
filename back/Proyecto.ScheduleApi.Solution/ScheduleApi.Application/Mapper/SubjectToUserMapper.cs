using ScheduleApi.Application.DTOs;
using ScheduleApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleApi.Application.Mapper
{
    public static class SubjectToUserMapper
    {
        public static SubjectToUserDTO FromEntity(SubjectToUser entity)
        {
            return new SubjectToUserDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                CourseId = entity.CourseId,
                HoraInicio = entity.HoraInicio
            };
        }

        public static SubjectToUser ToEntity(SubjectToUserDTO dto)
        {
            return new SubjectToUser
            {
                Id = dto.Id,
                UserId = dto.UserId,
                CourseId = dto.CourseId,
                HoraInicio = dto.HoraInicio
            };
        }

        public static SubjectToUser ToEntityWithId(SubjectToUserDTO dto)
        {
            return new SubjectToUser
            {
                Id = dto.Id,
                UserId = dto.UserId,
                CourseId = dto.CourseId,
                HoraInicio = dto.HoraInicio
            };
        }

        public static List<SubjectToUserDTO> FromEntityList(List<SubjectToUser> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<SubjectToUser> ToEntityList(List<SubjectToUserDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
