using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using SubjectsApi.Application.DTOs;
using SubjectsApi.Application.Interfaces;
using SubjectsApi.Application.Mappers;
using SubjectsApi.Domain.Entities;
using SubjectsApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SubjectsApi.Infrastructure.Repositories
{
    public class SubjectRepository(SubjectsDbContext context) : ISubject
    {
        private readonly List<string> validSubjects = new List<string>
        {
            "Español", "Espanol", "Matematicas", "Historia", "Geografia", "Sociales"
        };

        private readonly List<string> validWorkshops = new List<string>
        {
            "Computación", "Electricidad", "Dibujo técnico", "Cocina", "Corte y confección"
        };

        private readonly List<string> validWorkshopSeries = new List<string>
        {
            "I", "II", "III"
        };

        public async Task<Response> CreateAsync(SubjectDTO dto)
        {
            try
            {
                if (dto.Tipo != "Materia" && dto.Tipo != "Taller")
                    return new Response(false, "El tipo debe ser 'Materia' o 'Taller'.");

                if (dto.Tipo == "Materia" && !validSubjects.Contains(dto.Nombre))
                    return new Response(false, "La materia no es válida.");

                if (dto.Tipo == "Taller")
                {
                    string workshopName = dto.Nombre.Substring(0, dto.Nombre.LastIndexOf(' '));
                    if (!validWorkshops.Contains(workshopName))
                        return new Response(false, "El taller no es válido.");

                    string series = dto.Nombre.Substring(dto.Nombre.LastIndexOf(' ') + 1);
                    if (!validWorkshopSeries.Contains(series))
                        return new Response(false, "Los talleres deben ser seriados (I, II, III).");
                }

                var entity = SubjectMapper.ToEntity(dto);
                entity.FechaCreacion = DateTime.UtcNow;

                context.Subjects.Add(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Materia creada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear la materia");
            }
        }

        public async Task<Response> DeleteAsync(SubjectDTO dto)
        {
            try
            {
                var entity = await context.Subjects.FindAsync(dto.Id);
                if (entity is null)
                    return new Response(false, "Materia no encontrada");

                context.Subjects.Remove(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Materia eliminada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar la materia");
            }
        }

        public async Task<SubjectDTO> FindByIdAsync(int id)
        {
            try
            {
                var entity = await context.Subjects.FindAsync(id);
                if (entity is null) return null!;
                return SubjectMapper.FromEntity(entity);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al buscar la materia por ID");
            }
        }

        public async Task<IEnumerable<SubjectDTO>> GetAllAsync()
        {
            try
            {
                var entities = await context.Subjects.ToListAsync();
                return SubjectMapper.FromEntityList(entities);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return Enumerable.Empty<SubjectDTO>();
            }
        }

        public async Task<SubjectDTO> GetByAsync(Expression<Func<SubjectDTO, bool>> predicate)
        {
            try
            {
                var allSubjects = await context.Subjects.ToListAsync();
                var dtoList = SubjectMapper.FromEntityList(allSubjects);
                return dtoList.AsQueryable().FirstOrDefault(predicate)!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al aplicar el filtro de búsqueda");
            }
        }

        public async Task<SubjectDTO> GetByCode(string code)
        {
            try
            {
                var entity = await context.Subjects.FirstOrDefaultAsync(s => s.Codigo == code);

                if (entity is null)
                    return null!;

                return SubjectMapper.FromEntity(entity);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al buscar la materia por código");
            }
        }

        public async Task<IEnumerable<SubjectDTO>> GetManyByAsync(Expression<Func<SubjectDTO, bool>> predicate)
        {
            try
            {
                var allSubjects = await context.Subjects.ToListAsync();
                var dtoList = SubjectMapper.FromEntityList(allSubjects);
                return dtoList.AsQueryable().Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en metodo get many by async en el repositorio");
            }
        }

        public async Task<Response> UpdateAsync(SubjectDTO dto)
        {
            try
            {
                if (dto.Tipo != "Materia" && dto.Tipo != "Taller")
                    return new Response(false, "El tipo debe ser 'Materia' o 'Taller'.");

                if (dto.Tipo == "Materia" && !validSubjects.Contains(dto.Nombre))
                    return new Response(false, "La materia no es válida.");

                if (dto.Tipo == "Taller")
                {
                    string workshopName = dto.Nombre.Substring(0, dto.Nombre.LastIndexOf(' '));
                    if (!validWorkshops.Contains(workshopName))
                        return new Response(false, "El taller no es válido.");

                    string series = dto.Nombre.Substring(dto.Nombre.LastIndexOf(' ') + 1);
                    if (!validWorkshopSeries.Contains(series))
                        return new Response(false, "Los talleres deben ser seriados (I, II, III).");
                }

                var entity = await context.Subjects.FindAsync(dto.Id);
                if (entity is null)
                    return new Response(false, "Materia no encontrada");

                context.Entry(entity).State = EntityState.Detached;

                var updatedEntity = SubjectMapper.ToEntity(dto);
                updatedEntity.FechaActualizacion = DateTime.UtcNow;

                context.Subjects.Update(updatedEntity);
                await context.SaveChangesAsync();

                return new Response(true, "Materia actualizada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar la materia");
            }
        }
    }
}
