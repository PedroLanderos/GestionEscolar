using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly IAsistencia _asistenciaService;
        private readonly ICicloEscolar _cicloEscolarService;

        public AsistenciasController(IAsistencia asistenciaService, ICicloEscolar cicloEscolarService)
        {
            _asistenciaService = asistenciaService;
            _cicloEscolarService = cicloEscolarService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearAsistencia([FromBody] AsistenciaDTO dto)
        {
            var result = await _asistenciaService.CrearAsistencia(dto);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarAsistencia([FromBody] AsistenciaDTO dto)
        {
            var result = await _asistenciaService.ActualizarAsistencia(dto);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAsistencia(int id)
        {
            var result = await _asistenciaService.EliminarAsistencia(id);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerAsistencias()
        {
            try
            {
                var cicloEscolar = await _cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                // Filtrar las asistencias dentro del rango de fechas del ciclo escolar
                var asistencias = await _asistenciaService.GetBy(a =>
                    a.Fecha >= cicloEscolar.FechaInicio &&
                    a.Fecha <= cicloEscolar.FechaFin
                );

                var dtoList = asistencias.Select(AsistenciaMapper.FromEntity).ToList();
                return Ok(dtoList);
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
                var result = await _asistenciaService.ObtenerAsistenciaPorId(id);
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
                var cicloEscolar = await _cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                var asistencias = await _asistenciaService.GetBy(a =>
                    a.IdAlumno == idAlumno &&
                    a.Fecha >= cicloEscolar.FechaInicio &&
                    a.Fecha <= cicloEscolar.FechaFin
                );

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
                var cicloEscolar = await _cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                var asistencias = await _asistenciaService.GetBy(a =>
                    a.IdProfesor == idProfesor &&
                    a.Fecha.Date == fecha.Date &&
                    a.Fecha >= cicloEscolar.FechaInicio &&
                    a.Fecha <= cicloEscolar.FechaFin
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
                var cicloEscolar = await _cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                // Filtrar las asistencias por profesor dentro del ciclo escolar
                var asistencias = await _asistenciaService.GetBy(a =>
                    a.IdProfesor == idProfesor &&
                    a.Fecha >= cicloEscolar.FechaInicio &&
                    a.Fecha <= cicloEscolar.FechaFin
                );

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
                var cicloEscolar = await _cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                var asistencias = await _asistenciaService.GetBy(a =>
                    a.IdAlumno == idAlumno &&
                    a.Justificacion != null && a.Justificacion != string.Empty &&
                    a.Fecha >= cicloEscolar.FechaInicio &&
                    a.Fecha <= cicloEscolar.FechaFin
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
                var cicloEscolar = await _cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                var inasistencias = await _asistenciaService.GetBy(a =>
                    a.Asistio == false &&
                    string.IsNullOrEmpty(a.Justificacion) &&
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


        [HttpPost("profesor/{idProfesor}/fecha/{fecha}")]
        public async Task<IActionResult> ObtenerAsistenciasPorFechaYAlumnos(
        string idProfesor,
        DateTime fecha,
        [FromBody] List<string> idAlumnos)
        {
            try
            {
                var cicloEscolar = await _cicloEscolarService.GetBy(x => x.EsActual == true);
                if (cicloEscolar == null)
                {
                    return BadRequest("No se encontró un ciclo escolar activo.");
                }

                // Filtramos las asistencias por fecha, profesor y el conjunto de alumnos
                var asistencias = await _asistenciaService.GetBy(a =>
                    idAlumnos.Contains(a.IdAlumno!) &&   // Filtramos por los alumnos proporcionados
                    a.IdProfesor == idProfesor &&        // Filtramos por el profesor
                    a.Fecha.Date == fecha.Date &&        // Filtramos por la fecha exacta
                    a.Fecha >= cicloEscolar.FechaInicio &&  // Aseguramos que la fecha esté dentro del ciclo escolar
                    a.Fecha <= cicloEscolar.FechaFin
                );

                var dtoList = asistencias.Select(AsistenciaMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las asistencias por fecha y alumnos: {ex.Message}");
            }
        }
    }
}
