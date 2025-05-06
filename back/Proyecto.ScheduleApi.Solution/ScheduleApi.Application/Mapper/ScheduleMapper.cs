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
            var hora = TimeSpan.TryParse(entity.HoraInicio, out var parsedHora) ? parsedHora : TimeSpan.Zero;
            var turno = hora < new TimeSpan(12, 0, 0) ? "Matutino" : "Vespertino";

            return new ScheduleDTO(
                Id: entity.Id,
                IdUsuario: entity.IdUsuario,
                IdMateria: entity.IdMateria,
                HoraInicio: entity.HoraInicio ?? "",
                Dia: entity.Dia ?? "",
                Turno: turno
            );
        }

        public static Schedule ToEntity(ScheduleDTO dto)
        {
            return new Schedule
            {
                Id = dto.Id,
                IdUsuario = dto.IdUsuario,
                IdMateria = dto.IdMateria,
                HoraInicio = dto.HoraInicio,
                Dia = dto.Dia
                // Turno no se incluye porque no existe en la entidad
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
