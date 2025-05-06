using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs;
using ScheduleApi.Application.Interfaces;
using ScheduleApi.Application.Mapper;
using ScheduleApi.Domain.Entities;
using ScheduleApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ScheduleApi.Infrastructure.Repositories
{
    public class ScheduleRepository(ScheduleDbContext context) : ISchedule
    {
        public async Task<Response> Create(ScheduleDTO dto)
        {
            try
            {
                var entity = ScheduleMapper.ToEntity(dto);
                context.Schedules.Add(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Horario creado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear el horario");
            }
        }

        public async Task<Response> Update(ScheduleDTO dto)
        {
            try
            {
                var entity = await context.Schedules.FindAsync(dto.Id);
                if (entity is null)
                    return new Response(false, "Horario no encontrado");

                context.Entry(entity).State = EntityState.Detached;

                var updatedEntity = ScheduleMapper.ToEntity(dto);
                context.Schedules.Update(updatedEntity);
                await context.SaveChangesAsync();

                return new Response(true, "Horario actualizado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar el horario");
            }
        }

        public async Task<IEnumerable<Schedule>> GetBy(Expression<Func<ScheduleDTO, bool>> predicate)
        {
            try
            {
                var allSchedules = await context.Schedules.ToListAsync();
                var dtoList = ScheduleMapper.FromEntityList(allSchedules);
                var filtered = dtoList.AsQueryable().Where(predicate).ToList();
                return ScheduleMapper.ToEntityList(filtered);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return Enumerable.Empty<Schedule>();
            }
        }
    }
}
