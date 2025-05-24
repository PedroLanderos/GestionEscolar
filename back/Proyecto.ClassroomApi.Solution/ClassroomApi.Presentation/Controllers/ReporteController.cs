using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController(IReporte reporteService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var result = await reporteService.ObtenerReportes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var reporte = await reporteService.ObtenerReportePorId(id);
                if (reporte == null)
                    return NotFound("Reporte no encontrado");

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el reporte: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ReporteDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await reporteService.CrearReporte(dto);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ReporteDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del cuerpo y la URL no coinciden");

            var response = await reporteService.ActualizarReporte(dto);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await reporteService.EliminarReporte(id);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("grupo/{grupo}")]
        public async Task<IActionResult> ObtenerPorGrupo(string grupo)
        {
            try
            {
                var reportes = await reporteService.GetBy(r => r.Grupo != null && r.Grupo.Equals(grupo));
                if (!reportes.Any())
                    return NotFound($"No se encontraron reportes para el grupo {grupo}");

                var result = reportes.Select(ReporteMapper.FromEntity).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes del grupo {grupo}: {ex.Message}");
            }
        }

        [HttpGet("grupo/{grupo}/fechas")]
        public async Task<IActionResult> ObtenerPorGrupoYFechas(string grupo, [FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            try
            {
                var reportes = await reporteService.GetBy(r =>
                    r.Grupo != null &&
                    r.Grupo.Equals(grupo) &&
                    r.Fecha >= inicio &&
                    r.Fecha <= fin);

                if (!reportes.Any())
                    return NotFound($"No se encontraron reportes para el grupo {grupo} entre {inicio:yyyy-MM-dd} y {fin:yyyy-MM-dd}");

                var result = reportes.Select(ReporteMapper.FromEntity).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes del grupo {grupo} por fechas: {ex.Message}");
            }
        }

        [HttpGet("ciclo/{cicloEscolar}")]
        public async Task<IActionResult> ObtenerPorCicloEscolar(string cicloEscolar)
        {
            try
            {
                var reportes = await reporteService.GetBy(r =>
                    r.CicloEscolar != null &&
                    r.CicloEscolar.Equals(cicloEscolar));

                if (!reportes.Any())
                    return NotFound($"No se encontraron reportes para el ciclo escolar '{cicloEscolar}'");

                var result = reportes.Select(ReporteMapper.FromEntity).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes del ciclo escolar '{cicloEscolar}': {ex.Message}");
            }
        }


        [HttpGet("alumno/{idAlumno}")]
        public async Task<IActionResult> ObtenerPorAlumno(string idAlumno)
        {
            try
            {
                var reportes = await reporteService.GetBy(r => r.IdAlumno == idAlumno);

                if (!reportes.Any())
                    return NotFound($"No se encontraron reportes para el alumno con ID {idAlumno}");

                var result = reportes.Select(ReporteMapper.FromEntity).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes del alumno {idAlumno}: {ex.Message}");
            }
        }

        [HttpGet("tipo/{tipo}")]
        public async Task<IActionResult> ObtenerPorTipo(string tipo)
        {
            try
            {
                var reportes = await reporteService.GetBy(r =>
                    r.Tipo != null &&
                    r.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));

                if (!reportes.Any())
                    return NotFound($"No se encontraron reportes del tipo '{tipo}'");

                var result = reportes.Select(ReporteMapper.FromEntity).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes por tipo '{tipo}': {ex.Message}");
            }
        }

        [HttpGet("ciclo/{cicloEscolar}/alumno/{idAlumno}")]
        public async Task<IActionResult> ObtenerPorCicloYAlumno(string cicloEscolar, string idAlumno)
        {
            try
            {
                var reportes = await reporteService.GetBy(r =>
                    r.CicloEscolar != null && r.CicloEscolar.Equals(cicloEscolar) &&
                    r.IdAlumno == idAlumno);

                if (!reportes.Any())
                    return NotFound($"No se encontraron reportes para el alumno '{idAlumno}' en el ciclo '{cicloEscolar}'");

                var result = reportes.Select(ReporteMapper.FromEntity).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes por ciclo '{cicloEscolar}' y alumno '{idAlumno}': {ex.Message}");
            }
        }

        [HttpGet("alumno/{idAlumno}/fechas")]
        public async Task<IActionResult> ObtenerPorAlumnoYFechas(string idAlumno, [FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            try
            {
                var reportes = await reporteService.GetBy(r =>
                    r.IdAlumno == idAlumno &&
                    r.Fecha >= inicio &&
                    r.Fecha <= fin);

                if (!reportes.Any())
                    return NotFound($"No se encontraron reportes para el alumno '{idAlumno}' entre {inicio:yyyy-MM-dd} y {fin:yyyy-MM-dd}");

                var result = reportes.Select(ReporteMapper.FromEntity).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener reportes por alumno '{idAlumno}' y fechas: {ex.Message}");
            }
        }
    }
}
