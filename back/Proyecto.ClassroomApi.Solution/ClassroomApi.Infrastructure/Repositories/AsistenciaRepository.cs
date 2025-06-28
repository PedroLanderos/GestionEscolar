using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using ClassroomApi.Application.Services;
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
    public class AsistenciaRepository(ClassroomDbContext context, ICicloEscolar _ciclo, IReporte _reporte, ISchedule _schedule) : IAsistencia
    {
        public async Task<Response> CrearAsistencia(AsistenciaDTO dto)
        {
            try
            {
                var entity = AsistenciaMapper.ToEntity(dto);
                await context.Asistencias.AddAsync(entity);
                await context.SaveChangesAsync();

                if (dto.Asistio == false)
                {
                    try
                    {
                        var ciclo = await _ciclo.GetBy(x => x.EsActual == true);
                        if (ciclo == null)
                        {
                            return new Response(false, "No se encontró un ciclo escolar activo para crear el reporte.");
                        }

                        var horario = await _schedule.GetScheduleByUserId(dto.IdAlumno!);
                        if (horario == null)
                        {
                            return new Response(false, "No se encontró un horario asignado para el alumno.");
                        }

                        var gradoGrupo = $"{horario.Grado}{horario.Grupo}";

                        var reporte = new ReporteDTO
                        (
                            Id: 0,
                            Fecha: dto.Fecha,
                            IdAlumno: dto.IdAlumno!,
                            Grupo: gradoGrupo,
                            CicloEscolar: ciclo.Id,
                            Tipo: "Inasistencia"
                        );

                        var reporteResponse = await _reporte.CrearReporte(reporte);
                        if (!reporteResponse.Flag)
                        {
                            LogException.LogExceptions(new Exception("Error al crear el reporte: " + reporteResponse.Message));
                        }
                    }
                    catch (Exception exReporte)
                    {
                        LogException.LogExceptions(exReporte);
                    }
                }

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

                bool cambioInasistenciaAAsistencia = existing.Asistio == false && dto.Asistio == true;

                existing.Fecha = dto.Fecha;
                existing.Asistio = dto.Asistio;
                existing.Justificacion = dto.Justificacion;
                existing.IdAlumno = dto.IdAlumno;
                existing.IdProfesor = dto.IdProfesor;

                context.Asistencias.Update(existing);
                await context.SaveChangesAsync();

                if (cambioInasistenciaAAsistencia)
                {
                    try
                    {
                        var horario = await _schedule.GetScheduleByUserId(dto.IdAlumno!);
                        if (horario == null)
                        {
                            LogException.LogExceptions(new Exception("No se encontró horario asignado para el alumno al actualizar asistencia."));
                        }
                        else
                        {
                            var gradoGrupo = $"{horario.Grado}{horario.Grupo}";

                            var reporteDto = await _reporte.ObtenerReporteInasistenciaPorAlumnoFechaGrupoAsync(dto.IdAlumno!, dto.Fecha, gradoGrupo);

                            if (reporteDto != null)
                            {
                                var eliminarResponse = await _reporte.EliminarReporte(reporteDto.Id);
                                if (!eliminarResponse.Flag)
                                {
                                    LogException.LogExceptions(new Exception($"Error al eliminar reporte con Id {reporteDto.Id}: {eliminarResponse.Message}"));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException.LogExceptions(ex);
                    }
                }

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
