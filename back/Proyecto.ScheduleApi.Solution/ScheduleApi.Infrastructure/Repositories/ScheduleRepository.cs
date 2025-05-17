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
        public async Task<Response> AsignStudentToScheduleAsync(ScheduleToUserDTO dto)
        {
            try
            {
                var entity = ScheduleToUserMapper.ToEntity(dto);
                var response = context.ScheduleToUsers.Add(entity);
                await context.SaveChangesAsync();

                if (response is null) return new Response(false, "Error al asignar el horario al alumno");
                return new Response(true, "Horario asignado al alumno exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }

        public async Task<Response> AsignSubjectToScheduleAsync(SubjectToScheduleDTO subjectToScheduleDTO)
        {
            try
            {
                var entity = SubjectToScheduleMapper.ToEntity(subjectToScheduleDTO);
                var response = context.SubjectToSchedules.Add(entity);
                await context.SaveChangesAsync();

                if(response is null) return new Response(false, "Error al asignar la materia al horario");
                return new Response(true, "Materia asignada al horario exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al asignar materia a horario en el repositorio");
            }
        }

        public async Task<Response> CreateSchedule(ScheduleDTO schedule)
        {
            try
            {
                var entity = ScheduleMapper.ToEntity(schedule);
                var response = context.Schedules.Add(entity);
                await context.SaveChangesAsync();

                if(response is null) return new Response(false, "Error al crear horario");
                return new Response(true, "Horario creado exitosamente");

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear horario desde el repositorio");
            }
        }

        public async Task<Response> DeleteAsignStudentToScheduleAsync(int id)
        {
            try
            {
                var assingment = await context.ScheduleToUsers.FindAsync(id);
                if(assingment == null)
                    return new Response(false, "Asignacion no encontrada");
                context.ScheduleToUsers.Remove(assingment);
                await context.SaveChangesAsync();
                return new Response(true, "Asignacion eliminada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar una asignacion de horario desde el repositorio");
            }
        }

        public async Task<Response> DeleteAsignSubjectToScheduleAsync(int id)
        {
            try
            {
                var subjectToSchedule = await context.SubjectToSchedules.FindAsync(id);
                if(subjectToSchedule == null)
                    return new Response(false, "Asignacion no encontrada");

                context.SubjectToSchedules.Remove(subjectToSchedule);
                await context.SaveChangesAsync();
                return new Response(true, "Asignacion eliminada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar la asignacion al horario en el repositorio");
            }
        }

        public async Task<Response> DeleteScheduleAsync(int id)
        {
            try
            {
                var schedule = await context.Schedules.FindAsync(id);
                if (schedule == null)
                    return new Response(false, "Horario no encontrado");

                context.Schedules.Remove(schedule);
                await context.SaveChangesAsync();

                return new Response(true, "Horario eliminado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar el horario");
            }
        }

        public async Task<IEnumerable<SubjectToSchedule>> GetBy(Expression<Func<SubjectToSchedule, bool>> predicate)
        {
            try
            {
                var results = await context.SubjectToSchedules.Where(predicate).ToListAsync();
                if (results is null) return null!;
                return results;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en metodo GetBy en el repositorio");
            }
        }

        public async Task<IEnumerable<SubjectToSchedule>> GetFullSchedule(int id)
        {
            try
            {
                var results = await context.SubjectToSchedules
                .Where(s => s.IdHorario == id)
                .ToListAsync();

                return results!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener el horario completo en el repositorio");
            }
        }

        public async Task<ScheduleDTO> GetSchedule(int id)
        {
            try
            {
                var schedule = await context.Schedules.FindAsync(id);
                if (schedule is null) return null!;
                return ScheduleMapper.FromEntity(schedule);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Erro en get schedule en el repositorio");
            }
        }

        public async Task<IEnumerable<ScheduleDTO>> GetSchedules()
        {
            try
            {
                var schedules = await context.Schedules.ToListAsync();
                if (schedules is null) return null!;
                var scheduleDTOs = ScheduleMapper.FromEntityList(schedules);
                return scheduleDTOs;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener los horarios en repository");
            }
        }

        public async Task<Response> UpdateAsignmentAsync(SubjectToScheduleDTO subjectToScheduleDTO)
        {
            try
            {
                var existing = await context.SubjectToSchedules.FindAsync(subjectToScheduleDTO.Id);
                if (existing == null)
                    return new Response(false, "Asignación no encontrada");

                existing.IdMateria = subjectToScheduleDTO.IdMateria;
                existing.IdHorario = subjectToScheduleDTO.IdHorario;
                existing.HoraInicio = subjectToScheduleDTO.HoraInicio;
                existing.Dia = subjectToScheduleDTO.Dia;

                context.SubjectToSchedules.Update(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Asignación actualizada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar la asignación");
            }
        }

        public async Task<Response> UpdateAsignStudentToScheduleAsync(ScheduleToUserDTO dto)
        {
            try
            {
                var existing = await context.ScheduleToUsers.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Asignación no encontrada");

                existing.IdSchedule = dto.IdSchedule ?? 0;
                existing.IdUser = dto.IdUser;
                context.ScheduleToUsers.Update(existing);
                await context.SaveChangesAsync();
                return new Response(true, "Asignación actualizada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar la asignacion de horario en repositorio");
            }
        }

        public async Task<Response> UpdateScheduleAsync(ScheduleDTO schedule)
        {
            try
            {
                var existing = await context.Schedules.FindAsync(schedule.Id);
                if (existing == null)
                    return new Response(false, "Horario no encontrado");

                existing.Grado = schedule.Grado;
                existing.Grupo = schedule.Grupo;

                context.Schedules.Update(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Horario actualizado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar el horario");
            }
        }

    }
}
