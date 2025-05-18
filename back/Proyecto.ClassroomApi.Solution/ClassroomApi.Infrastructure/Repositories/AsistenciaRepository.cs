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
    public class AsistenciaRepository(ClassroomDbContext context) : IAsistencia
    {
        public async Task<Response> CrearAsistencia(AsistenciaDTO dto)
        {
            try
            {
                var entity = AsistenciaMapper.ToEntity(dto);
                await context.Asistencias.AddAsync(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Asistencia registrada correctamente.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al registrar la asistencia.");
            }
        }

        public async Task<Response> ActualizarAsistencia(AsistenciaDTO dto)
        {
            try
            {
                var existing = await context.Asistencias.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Asistencia no encontrada.");

                existing.Fecha = dto.Fecha;
                existing.Asistio = dto.Asistio;
                existing.Justificacion = dto.Justificacion;
                existing.IdAlumno = dto.IdAlumno;
                existing.IdProfesor = dto.IdProfesor;

                context.Asistencias.Update(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Asistencia actualizada correctamente.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar la asistencia.");
            }
        }

        public async Task<Response> EliminarAsistencia(int id)
        {
            try
            {
                var asistencia = await context.Asistencias.FindAsync(id);
                if (asistencia == null)
                    return new Response(false, "Asistencia no encontrada.");

                context.Asistencias.Remove(asistencia);
                await context.SaveChangesAsync();

                return new Response(true, "Asistencia eliminada correctamente.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar la asistencia.");
            }
        }

        public async Task<IEnumerable<AsistenciaDTO>> ObtenerAsistencias()
        {
            try
            {
                var asistencias = await context.Asistencias.ToListAsync();
                return AsistenciaMapper.FromEntityList(asistencias);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener las asistencias.");
            }
        }

        public async Task<AsistenciaDTO> ObtenerAsistenciaPorId(int id)
        {
            try
            {
                var asistencia = await context.Asistencias.FindAsync(id);
                if (asistencia == null)
                    return null!;

                return AsistenciaMapper.FromEntity(asistencia);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener la asistencia por ID.");
            }
        }

        public async Task<IEnumerable<Asistencia>> GetBy(Expression<Func<Asistencia, bool>> predicate)
        {
            try
            {
                return await context.Asistencias.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al filtrar las asistencias.");
            }
        }
    }
}
