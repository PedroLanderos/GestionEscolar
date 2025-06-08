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
                context.ScheduleToUsers.Add(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Horario asignado al alumno exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al asignar el horario al alumno");
            }
        }


        public async Task<Response> AsignSubjectToScheduleAsync(SubjectToScheduleDTO subjectToScheduleDTO)
        {
            try
            {
                
                var traslape = await context.SubjectToSchedules
                    .AnyAsync(s =>
                        s.IdMateria == subjectToScheduleDTO.IdMateria &&
                        s.Dia == subjectToScheduleDTO.Dia &&
                        s.HoraInicio == subjectToScheduleDTO.HoraInicio &&
                        s.IdHorario != subjectToScheduleDTO.IdHorario); 

                if (traslape)
                {
                    return new Response(false, "Ya existe una asignación para esta materia en otro horario en el mismo día y hora. No se permite el traslape.");
                }

                // Si no hay traslape, se asigna la materia normalmente
                var entity = SubjectToScheduleMapper.ToEntity(subjectToScheduleDTO);
                var response = context.SubjectToSchedules.Add(entity);
                await context.SaveChangesAsync();

                if (response is null)
                    return new Response(false, "Error al asignar la materia al horario");

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
                if (schedule.Grupo != "A" && schedule.Grupo != "B")
                    return new Response(false, "Solo se permiten los grupos A y B.");

                if (schedule.Grado < 1 || schedule.Grado > 3)
                    return new Response(false, "Solo se permiten grados del 1 al 3.");

                var countSchedulesForGrade = await context.Schedules
                    .CountAsync(s => s.Grado == schedule.Grado);

                if (countSchedulesForGrade >= 2)
                    return new Response(false, $"Ya existen 2 horarios para el grado {schedule.Grado}. No se permiten más.");

                var exists = await context.Schedules
                    .AnyAsync(s => s.Grado == schedule.Grado && s.Grupo == schedule.Grupo);

                if (exists)
                    return new Response(false, $"Ya existe un horario para el grado {schedule.Grado} y grupo {schedule.Grupo}.");

                var entity = ScheduleMapper.ToEntity(schedule);
                var response = context.Schedules.Add(entity);
                await context.SaveChangesAsync();

                if (response is null) return new Response(false, "Error al crear horario");
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

        public async Task<IEnumerable<SubjectToScheduleDTO>> GetScheduleForTeacherAsync(string idUsuario)
        {
            try
            {
                var results = await context.SubjectToSchedules.Where(s => s.IdMateria != null && s.IdMateria.EndsWith($"-{idUsuario}")).ToListAsync();
                return SubjectToScheduleMapper.FromEntityList(results);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al iobtener el horario del docente en el repositorio");
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

                
                var traslape = await context.SubjectToSchedules
                    .AnyAsync(s =>
                        s.Id != subjectToScheduleDTO.Id && 
                        s.IdMateria == subjectToScheduleDTO.IdMateria &&
                        s.Dia == subjectToScheduleDTO.Dia &&
                        s.HoraInicio == subjectToScheduleDTO.HoraInicio &&
                        s.IdHorario != subjectToScheduleDTO.IdHorario); 

                if (traslape)
                {
                    return new Response(false, "Ya existe una asignación para esta materia en otro horario en el mismo día y hora. No se permite el traslape.");
                }

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

                if (schedule.Grupo != "A" && schedule.Grupo != "B")
                    return new Response(false, "Solo se permiten los grupos A y B.");

                if (schedule.Grado < 1 || schedule.Grado > 3)
                    return new Response(false, "Solo se permiten grados del 1 al 3.");

                var countSchedulesForGrade = await context.Schedules
                    .CountAsync(s => s.Grado == schedule.Grado && s.Id != schedule.Id);

                if (countSchedulesForGrade >= 2)
                    return new Response(false, $"Ya existen 2 horarios para el grado {schedule.Grado}. No se permiten más.");

                var exists = await context.Schedules
                    .AnyAsync(s => s.Grado == schedule.Grado && s.Grupo == schedule.Grupo && s.Id != schedule.Id);

                if (exists)
                    return new Response(false, $"Ya existe un horario para el grado {schedule.Grado} y grupo {schedule.Grupo}.");

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

        public async Task<IEnumerable<SubjectToScheduleDTO>> GetScheduleForStudentAsync(string idUsuario)
        {
            try
            {
                var asignacion = await context.ScheduleToUsers
                    .FirstOrDefaultAsync(a => a.IdUser == idUsuario);

                if (asignacion == null)
                    return new List<SubjectToScheduleDTO>();

                // Usar método existente para obtener las materias
                var materias = await GetFullSchedule(asignacion.IdSchedule);
                return SubjectToScheduleMapper.FromEntityList(materias.ToList());
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener el horario del alumno en el repositorio");
            }
        }

        public async Task<IEnumerable<string>> GetStudentIdsBySubjectAndScheduleAsync(string materiaProfesor, string horario)
        {
            try
            {
                // Primero obtener el horario con grado y grupo iguales al parámetro "horario"
                // horario tiene formato "1A", se separa en grado y grupo
                if (string.IsNullOrEmpty(horario) || horario.Length < 2)
                    return Enumerable.Empty<string>();

                int grado;
                if (!int.TryParse(horario.Substring(0, horario.Length - 1), out grado))
                    return Enumerable.Empty<string>();

                string grupo = horario.Substring(horario.Length - 1);

                // Buscar el idHorario con esos datos
                var schedule = await context.Schedules
                    .FirstOrDefaultAsync(s => s.Grado == grado && s.Grupo == grupo);

                if (schedule == null)
                    return Enumerable.Empty<string>();

                // Verificar si la materia existe en SubjectToSchedules para ese IdHorario
                var subjectExists = await context.SubjectToSchedules
                    .AnyAsync(sts => sts.IdMateria == materiaProfesor && sts.IdHorario == schedule.Id);

                if (!subjectExists)
                    return Enumerable.Empty<string>();

                // Buscar todos los asignados al horario con IdSchedule == schedule.Id
                var studentsAssignments = await context.ScheduleToUsers
                    .Where(stu => stu.IdSchedule == schedule.Id)
                    .ToListAsync();

                // Retornar los IdUser de los alumnos asignados
                return studentsAssignments.Select(sa => sa.IdUser!).ToList();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener alumnos por materia y horario");
            }
        }

        public async Task<ScheduleDTO?> GetScheduleByUserIdAsync(string idUser)
        {
            try
            {
                // Buscar la asignación del usuario en ScheduleToUsers
                var assignment = await context.ScheduleToUsers
                    .FirstOrDefaultAsync(s => s.IdUser == idUser);

                if (assignment == null)
                    return null; // No asignado a ningún horario

                // Obtener el horario asignado con IdSchedule
                var schedule = await context.Schedules.FindAsync(assignment.IdSchedule);

                if (schedule == null)
                    return null; // Horario no encontrado

                // Mapear a DTO y devolver
                return ScheduleMapper.FromEntity(schedule);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener el horario por IdUser en el repositorio");
            }
        }

        public async Task<Response> CreateWorkshopAsync(SubjectToUserDTO workshop)
        {
            try
            {
                var entity = SubjectToUserMapper.ToEntity(workshop); 
                context.SubjectToUsers.Add(entity); 
                await context.SaveChangesAsync(); 

                return new Response(true, "Taller asignado exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); 
                return new Response(false, "Error al asignar el taller");
            }
        }


        public async Task<Response> UpdateWorkshopAsync(SubjectToUserDTO workshop)
        {
            try
            {
                var existing = await context.SubjectToUsers.FindAsync(workshop.Id);
                if (existing == null)
                    return new Response(false, "Taller no encontrado");

                
                existing.UserId = workshop.UserId;
                existing.CourseId = workshop.CourseId;
                existing.HoraInicio = workshop.HoraInicio;
                existing.Dia = workshop.Dia;

                context.SubjectToUsers.Update(existing); 
                await context.SaveChangesAsync(); 

                return new Response(true, "Taller actualizado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); 
                return new Response(false, "Error al actualizar el taller");
            }
        }


        public async Task<Response> DeleteWorkshopAsync(int id)
        {
            try
            {
                var workshop = await context.SubjectToUsers.FindAsync(id);
                if (workshop == null)
                    return new Response(false, "Taller no encontrado");

                context.SubjectToUsers.Remove(workshop); 
                await context.SaveChangesAsync(); 

                return new Response(true, "Taller eliminado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); 
                return new Response(false, "Error al eliminar el taller");
            }
        }


        public async Task<IEnumerable<SubjectToUserDTO>> GetWorkshopsAsync()
        {
            try
            {
                var workshops = await context.SubjectToUsers.ToListAsync();
                return SubjectToUserMapper.FromEntityList(workshops);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); 
                throw new Exception("Error al obtener los talleres");
            }
        }


        public async Task<SubjectToUserDTO?> GetWorkshopByIdAsync(int id)
        {
            try
            {
                var workshop = await context.SubjectToUsers.FindAsync(id); 
                if (workshop == null) return null; 
                return SubjectToUserMapper.FromEntity(workshop); 
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); 
                throw new Exception("Error al obtener el taller por ID");
            }
        }


        public async Task<IEnumerable<SubjectToUser>> GetWorkShopBy(Expression<Func<SubjectToUser, bool>> predicate)
        {
            try
            {
                var results = await context.SubjectToUsers.Where(predicate).ToListAsync(); 
                if (results == null) return new List<SubjectToUser>(); 
                return results;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); 
                throw new Exception("Error al obtener los talleres por filtro");
            }
        }

    }
}