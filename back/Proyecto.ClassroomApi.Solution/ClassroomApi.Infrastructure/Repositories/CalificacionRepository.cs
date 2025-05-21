using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using ClassroomApi.Domain.Entities;
using ClassroomApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassroomApi.Infrastructure.Repositories
{
    public class CalificacionRepository : ICalificacion
    {
        private readonly ClassroomDbContext context;

        public CalificacionRepository(ClassroomDbContext context)
        {
            this.context = context;
        }

        public async Task<Response> AgregarCalificacion(CalificacionDTO dto)
        {
            try
            {
                if (dto.CalificacionFinal < 0m || dto.CalificacionFinal > 10m)
                    return new Response(false, "La calificación debe estar entre 0 y 10.");

                // Validar que IdCiclo no sea null ni vacío
                if (string.IsNullOrEmpty(dto.IdCiclo))
                    return new Response(false, "El IdCiclo es obligatorio para crear la calificación.");

                // Obtener ciclo escolar relacionado
                var ciclo = await context.CiclosEscolares.FindAsync(dto.IdCiclo);
                if (ciclo == null)
                    return new Response(false, "El ciclo escolar indicado no existe.");

                // Fecha límite para crear calificación: FechaRegistroCalificaciones + 3 días
                var fechaLimite = ciclo.FechaRegistroCalificaciones.AddDays(3);
                var fechaActual = DateTime.Now;

                if (fechaActual > fechaLimite)
                    return new Response(false, $"No se puede crear la calificación. La fecha límite fue {fechaLimite:yyyy-MM-dd}.");

                var entity = CalificacionMapper.ToEntity(dto);
                await context.Calificaciones.AddAsync(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Calificación creada exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear la calificación");
            }
        }


        public async Task<Response> ActualizarCalificacion(CalificacionDTO dto)
        {
            try
            {
                if (dto.CalificacionFinal < 0m || dto.CalificacionFinal > 10m)
                    return new Response(false, "La calificación debe estar entre 0 y 10.");

                if (string.IsNullOrEmpty(dto.IdCiclo))
                    return new Response(false, "El IdCiclo es obligatorio para actualizar la calificación.");

                var ciclo = await context.CiclosEscolares.FindAsync(dto.IdCiclo);
                if (ciclo == null)
                    return new Response(false, "El ciclo escolar indicado no existe.");

                var fechaLimite = ciclo.FechaRegistroCalificaciones.AddDays(3);
                var fechaActual = DateTime.Now;

                if (fechaActual > fechaLimite)
                    return new Response(false, $"No se puede actualizar la calificación. La fecha límite fue {fechaLimite:yyyy-MM-dd}.");

                var existing = await context.Calificaciones.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Calificación no encontrada");

                existing.IdMateria = dto.IdMateria;
                existing.IdAlumno = dto.IdAlumno;
                existing.CalificacionFinal = dto.CalificacionFinal;
                existing.Comentarios = dto.Comentarios;
                existing.IdCiclo = dto.IdCiclo;

                context.Calificaciones.Update(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Calificación actualizada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar la calificación");
            }
        }


        public async Task<Response> EliminarCalificacion(CalificacionDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.IdCiclo))
                    return new Response(false, "El IdCiclo es obligatorio para eliminar la calificación.");

                var ciclo = await context.CiclosEscolares.FindAsync(dto.IdCiclo);
                if (ciclo == null)
                    return new Response(false, "El ciclo escolar indicado no existe.");

                var fechaLimite = ciclo.FechaRegistroCalificaciones.AddDays(3);
                var fechaActual = DateTime.Now;

                if (fechaActual > fechaLimite)
                    return new Response(false, $"No se puede eliminar la calificación. La fecha límite fue {fechaLimite:yyyy-MM-dd}.");

                var existing = await context.Calificaciones.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Calificación no encontrada");

                context.Calificaciones.Remove(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Calificación eliminada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar la calificación");
            }
        }

        public async Task<IEnumerable<CalificacionDTO>> ObtenerCalificaciones()
        {
            try
            {
                var entities = await context.Calificaciones.ToListAsync();
                return CalificacionMapper.FromEntityList(entities);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener las calificaciones");
            }
        }

        public async Task<CalificacionDTO> ObtenerCalificacionPorId(int id)
        {
            try
            {
                var entity = await context.Calificaciones.FindAsync(id);
                if (entity == null)
                    return null!;

                return CalificacionMapper.FromEntity(entity);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener calificación por id");
            }
        }

        public async Task<IEnumerable<Calificacion>> GetManyBy(Expression<Func<Calificacion, bool>> predicate)
        {
            try
            {
                return await context.Calificaciones.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener calificaciones con filtro");
            }
        }
    }
}
