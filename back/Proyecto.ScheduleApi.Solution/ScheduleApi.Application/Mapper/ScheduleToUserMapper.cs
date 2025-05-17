using ScheduleApi.Application.DTOs;
using ScheduleApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleApi.Application.Mapper
{
    public static class ScheduleToUserMapper
    {
        public static ScheduleToUserDTO FromEntity(ScheduleToUser entity)
        {
            return new ScheduleToUserDTO
            {
                Id = entity.Id,
                IdSchedule = entity.IdSchedule,
                IdUser = entity.IdUser
            };
        }

        public static ScheduleToUser ToEntity(ScheduleToUserDTO dto)
        {
            return new ScheduleToUser
            {
                Id = dto.Id,
                IdSchedule = dto.IdSchedule ?? 0,
                IdUser = dto.IdUser
            };
        }

        public static List<ScheduleToUserDTO> FromEntityList(List<ScheduleToUser> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<ScheduleToUser> ToEntityList(List<ScheduleToUserDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
