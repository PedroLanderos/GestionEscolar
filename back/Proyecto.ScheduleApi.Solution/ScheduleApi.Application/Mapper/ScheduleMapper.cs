using ScheduleApi.Application.DTOs;
using ScheduleApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleApi.Application.Mapper
{
    public static class ScheduleMapper
    {
        public static ScheduleDTO FromEntity(Schedule entity)
        {
            return new ScheduleDTO(
                Id: entity.Id,
                Grado: entity.Grado ?? 0,
                Grupo: entity.Grupo!
            );
        }

        public static Schedule ToEntity(ScheduleDTO dto)
        {
            return new Schedule
            {
                Id = dto.Id,
                Grado = dto.Grado,
                Grupo = dto.Grupo
            };
        }

        public static List<ScheduleDTO> FromEntityList(List<Schedule> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<Schedule> ToEntityList(List<ScheduleDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
