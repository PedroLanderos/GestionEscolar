using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Mapper;
using ClassroomApi.Domain.Entities;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClassroomApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Logs;

namespace ClassroomApi.Infrastructure.Repositories
{
    
    public class ClassroomRepository<T> : IClassroom<T> where T : class 
    {
        private readonly ClassroomDbContext _context;

        public ClassroomRepository(ClassroomDbContext context)
        {
            _context = context;
        }

        public async Task<Response> RegisterAttendance(AsistenciaDTO asistenciaDTO)
        {
            try
            {
                var entity = AsistenciaMapper.ToEntity(asistenciaDTO);
                _context.Asistencias.Add(entity);
                await _context.SaveChangesAsync();

                return new Response(true, "Asistencia registrada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al registrar la asistencia");
            }
        }

        public async Task<Response> RegisterGrade(CalificacionDTO calificacionDTO)
        {
            try
            {
                var entity = CalificacionMapper.ToEntity(calificacionDTO);
                _context.Calificaciones.Add(entity);
                await _context.SaveChangesAsync();

                return new Response(true, "Calificación registrada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al registrar la calificación");
            }
        }

        public async Task<Response> GenerateReport(ReporteDTO reporteDTO)
        {
            try
            {
                var entity = ReporteMapper.ToEntity(reporteDTO);
                _context.Reportes.Add(entity);
                await _context.SaveChangesAsync();

                return new Response(true, "Reporte generado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al generar el reporte");
            }
        }

        public async Task<Response> RegisterSanction(SancionDTO sancionDTO)
        {
            try
            {
                var entity = SancionMapper.ToEntity(sancionDTO);
                _context.Sanciones.Add(entity);
                await _context.SaveChangesAsync();

                return new Response(true, "Sanción registrada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al registrar la sanción");
            }
        }

        public async Task<IEnumerable<ReporteDTO>> GetReportsByStudent(int studentId)
        {
            try
            {
                var reports = await _context.Reportes
                    .Where(r => r.IdAlumno == studentId) 
                    .ToListAsync();

                return ReporteMapper.FromEntityList(reports);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return Enumerable.Empty<ReporteDTO>();
            }
        }

        public async Task<T> GetByAsync(Expression<Func<T, bool>> predicate) 
        {
            try
            {
                var entity = await _context.Set<T>().FirstOrDefaultAsync(predicate);
                return entity!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al aplicar el filtro de búsqueda");
            }
        }
    }
}
