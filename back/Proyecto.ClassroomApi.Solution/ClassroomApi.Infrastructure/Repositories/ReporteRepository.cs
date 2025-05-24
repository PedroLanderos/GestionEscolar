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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassroomApi.Infrastructure.Repositories
{
    public class ReporteRepository(ClassroomDbContext context) : IReporte
    {
        public async Task<Response> CrearReporte(ReporteDTO dto)
        {
            try
            {
                var entity = ReporteMapper.ToEntity(dto);
                await context.Reportes.AddAsync(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Reporte creado exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear el reporte");
            }
        }

        public async Task<Response> ActualizarReporte(ReporteDTO dto)
        {
            try
            {
                var existing = await context.Reportes.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Reporte no encontrado");

                existing.Fecha = dto.Fecha;
                existing.IdAlumno = dto.IdAlumno;
                existing.Grupo = dto.Grupo;
                existing.CicloEscolar = dto.CicloEscolar;
                existing.idHorario = dto.IdHorario;
                existing.Tipo = dto.Tipo;

                context.Reportes.Update(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Reporte actualizado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar el reporte");
            }
        }

        public async Task<Response> EliminarReporte(int id)
        {
            try
            {
                var reporte = await context.Reportes.FindAsync(id);
                if (reporte == null)
                    return new Response(false, "Reporte no encontrado");

                context.Reportes.Remove(reporte);
                await context.SaveChangesAsync();

                return new Response(true, "Reporte eliminado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar el reporte");
            }
        }

        public async Task<IEnumerable<ReporteDTO>> ObtenerReportes()
        {
            try
            {
                var reportes = await context.Reportes.ToListAsync();
                return ReporteMapper.FromEntityList(reportes);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener los reportes");
            }
        }

        public async Task<ReporteDTO> ObtenerReportePorId(int id)
        {
            try
            {
                var reporte = await context.Reportes.FindAsync(id);
                if (reporte == null)
                    return null!;

                var dto = ReporteMapper.FromEntity(reporte);
                return ReporteMapper.FromEntity(reporte);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en obtener reporte por id en repositorio");
            }
        }

        public async Task<IEnumerable<Reporte>> GetBy(Expression<Func<Reporte, bool>> predicate)
        {
            try
            {
                return await context.Reportes.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al filtrar los reportes");
            }
        }

        public async Task<ReporteDTO?> ObtenerReporteInasistenciaPorAlumnoFechaGrupoAsync(string idAlumno, DateTime fecha, string grupo)
        {
            try
            {
                var reportes = await GetBy(r =>
                    r.IdAlumno == idAlumno &&
                    r.Fecha.Date == fecha.Date &&
                    r.Grupo == grupo &&
                    r.Tipo == "Inasistencia");

                var reporte = reportes.FirstOrDefault();

                if (reporte == null)
                    return null;

                return ReporteMapper.FromEntity(reporte);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener reporte de inasistencia por alumno, fecha y grupo en repositorio");
            }
        }
    }
}
