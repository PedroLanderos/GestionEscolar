using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ClassroomApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController(IAsistencia asistenciaService, ICicloEscolar cicloEscolarService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CrearAsistencia([FromBody] AsistenciaDTO dto)
        {
            var result = await asistenciaService.CrearAsistencia(dto);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarAsistencia([FromBody] AsistenciaDTO dto)
        {
            var result = await asistenciaService.ActualizarAsistencia(dto);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAsistencia(int id)
        {
            var result = await asistenciaService.EliminarAsistencia(id);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerAsistencias()
        {
            try
            {
                var result = await asistenciaService.ObtenerAsistencias();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las asistencias: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerAsistenciaPorId(int id)
        {
            try
            {
                var result = await asistenciaService.ObtenerAsistenciaPorId(id);
                if (result == null)
                    return NotFound("Asistencia no encontrada");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la asistencia: {ex.Message}");
            }
        }

        [HttpGet("alumno/{idAlumno}")]
        public async Task<IActionResult> ObtenerAsistenciasPorAlumno(string idAlumno)
        {
            try
            {
                var asistencias = await asistenciaService.GetBy(a => a.IdAlumno == idAlumno);
                var dtoList = asistencias.Select(AsistenciaMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener asistencias del alumno: {ex.Message}");
            }
        }

        [HttpGet("profesor/{idProfesor}/fecha/{fecha}")]
        public async Task<IActionResult> ObtenerAsistenciasPorProfesorYFecha(string idProfesor, DateTime fecha)
        {
            try
            {
                var asistencias = await asistenciaService.GetBy(a =>
                    a.IdProfesor == idProfesor &&
                    a.Fecha.Date == fecha.Date
                );

                var dtoList = asistencias.Select(AsistenciaMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener asistencias por profesor y fecha: {ex.Message}");
            }
        }

        [HttpGet("profesor/{idProfesor}")]
        public async Task<IActionResult> ObtenerAsistenciasPorProfesor(string idProfesor)
        {
            try
            {
                var asistencias = await asistenciaService.GetBy(a => a.IdProfesor == idProfesor);
                var dtoList = asistencias.Select(AsistenciaMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener asistencias del profesor: {ex.Message}");
            }
        }

        [HttpGet("alumno/{idAlumno}/justificadas")]
        public async Task<IActionResult> ObtenerAsistenciasJustificadasPorAlumno(string idAlumno)
        {
            try
            {
                var asistencias = await asistenciaService.GetBy(a =>
                    a.IdAlumno == idAlumno && a.Justificacion != null && a.Justificacion != string.Empty
                );

                var dtoList = asistencias.Select(AsistenciaMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener asistencias justificadas del alumno: {ex.Message}");
            }
        }

        [HttpGet("inasistencias/ciclo-escolar")]
        public async Task<IActionResult> ObtenerInasistenciasCicloEscolar()
        {
            try
            {
                // Obtener el ciclo escolar activo
                var cicloEscolar = await cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                // Filtrar las asistencias dentro del rango de fechas del ciclo escolar
                var inasistencias = await asistenciaService.GetBy(a =>
                    a.Asistio == false &&
                    a.Justificacion == null &&
                    a.Fecha >= cicloEscolar.FechaInicio &&
                    a.Fecha <= cicloEscolar.FechaFin
                );

                var result = inasistencias.Select(AsistenciaMapper.FromEntity).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las inasistencias: {ex.Message}");
            }
        }
    }
}
