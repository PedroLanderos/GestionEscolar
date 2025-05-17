using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using SubjectsApi.Application.DTOs;
using SubjectsApi.Application.Interfaces;
using SubjectsApi.Application.Mappers;
using SubjectsApi.Application.Services;
using SubjectsApi.Domain.Entities;
using SubjectsApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SubjectsApi.Infrastructure.Repositories
{
    public class SubjectAssignmentRepository(SubjectsDbContext context, IUser _user) : ISubjectAssignment
    {
        public async Task<Response> CreateAsignmnet(SubjectAssignmentDTO dto)
        {
            try
            {
                var docente = await _user.ObtenerDocente(dto.UserId!);

                if (docente is null)
                    return new Response(false, "Usuario no encontrado o no es un docente");

                var asignacionesDocente = await context.SubjectAssignments
                    .CountAsync(a => a.UserId == dto.UserId);

                if (asignacionesDocente >= 4)
                    return new Response(false, "El docente ya tiene el número máximo de materias asignadas (4)");

                var idGenerado = $"{dto.SubjectId}-{dto.UserId}";
                dto.Id = idGenerado;

                var existe = await context.SubjectAssignments.AnyAsync(a => a.Id == idGenerado);
                if (existe)
                    return new Response(false, "Ya existe una asignación para esa materia con ese docente");

                var entity = SubjectAssignmentMapper.ToEntity(dto);
                entity.FechaCreacion = DateTime.UtcNow;

                context.SubjectAssignments.Add(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Asignación creada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear la asignación");
            }
        }


        public async Task<Response> DeleteAsignmnet(string id)
        {
            try
            {
                var entity = await context.SubjectAssignments.FindAsync(id.ToString());
                if (entity is null)
                    return new Response(false, "Asignación no encontrada");

                context.SubjectAssignments.Remove(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Asignación eliminada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar la asignación");
            }
        }

        public async Task<IEnumerable<SubjectAssignmentDTO>> GetAsignmnets()
        {
            try
            {
                var entities = await context.SubjectAssignments.ToListAsync();
                return SubjectAssignmentMapper.FromEntityList(entities);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return Enumerable.Empty<SubjectAssignmentDTO>();
            }
        }

       
        public async Task<IEnumerable<SubjectAssignmentDTO>> GetAssignmentByGrade(int grado)
        {
            try
            {
                var subjectsIds = await context.Subjects.Where(s => s.Grado == grado).Select(s => s.Codigo).ToListAsync();

                var assingments = await context.SubjectAssignments.Where(a => subjectsIds.Contains(a.SubjectId!)).ToListAsync();

                return SubjectAssignmentMapper.FromEntityList(assingments);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en get assingment by id en repositorio");
            }
        }

        public async Task<IEnumerable<SubjectAssignment>> GetBy(Expression<Func<SubjectAssignment, bool>> predicate)
        {
            try
            {
                return await context.SubjectAssignments.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al aplicar filtro en asignaciones");
            }
        }

        public async Task<SubjectAssignmentDTO> GetById(string id)
        {
            try
            {
                var entity = await context.SubjectAssignments.FindAsync(id.ToString());
                return entity is not null ? SubjectAssignmentMapper.FromEntity(entity) : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al buscar la asignación por ID");
            }
        }

        public async Task<Response> UpdateAsignmnet(SubjectAssignmentDTO dto)
        {
            try
            {
                var entity = await context.SubjectAssignments.FindAsync(dto.Id);
                if (entity is null)
                    return new Response(false, "Asignación no encontrada");

                context.Entry(entity).State = EntityState.Detached;

                var updatedEntity = SubjectAssignmentMapper.ToEntity(dto);
                updatedEntity.FechaActualizacion = DateTime.UtcNow;

                context.SubjectAssignments.Update(updatedEntity);
                await context.SaveChangesAsync();

                return new Response(true, "Asignación actualizada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar la asignación");
            }
        }
    }
}