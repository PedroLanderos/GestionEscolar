using ScheduleApi.Application.DTOs;
using ScheduleApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleApi.Application.Mapper
{
    public static class SubjectToScheduleMapper
    {
        public static SubjectToScheduleDTO FromEntity(SubjectToSchedule entity)
        {
            return new SubjectToScheduleDTO
            {
                Id = entity.Id,
                IdMateria = entity.IdMateria,
                IdHorario = entity.IdHorario,
                HoraInicio = entity.HoraInicio,
                Dia = entity.Dia
            };
        }

        public static SubjectToSchedule ToEntity(SubjectToScheduleDTO dto)
        {
            return new SubjectToSchedule
            {
                Id = dto.Id,
                IdMateria = dto.IdMateria,
                IdHorario = dto.IdHorario,
                HoraInicio = dto.HoraInicio,
                Dia = dto.Dia
            };
        }

        public static List<SubjectToScheduleDTO> FromEntityList(List<SubjectToSchedule> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        public static List<SubjectToSchedule> ToEntityList(List<SubjectToScheduleDTO> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}
