using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionController : ControllerBase
    {
        private readonly ICalificacion calificacionService;

        public CalificacionController(ICalificacion calificacionService)
        {
            this.calificacionService = calificacionService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var result = await calificacionService.ObtenerCalificaciones();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener calificaciones: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var calificacion = await calificacionService.ObtenerCalificacionPorId(id);
                if (calificacion == null)
                    return NotFound("Calificación no encontrada");

                return Ok(calificacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la calificación: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CalificacionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await calificacionService.AgregarCalificacion(dto);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CalificacionDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del cuerpo y la URL no coinciden");

            var response = await calificacionService.ActualizarCalificacion(dto);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            // DTO temporal solo con Id para eliminar
            var dto = new CalificacionDTO(id, string.Empty, null, 0m, null, string.Empty);

            var response = await calificacionService.EliminarCalificacion(dto);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("aprobatorias/materia/{idMateria}")]
        public async Task<IActionResult> ObtenerAprobatoriasPorMateria(string idMateria)
        {
            try
            {
                var calificaciones = await calificacionService.GetManyBy(c => c.IdMateria == idMateria && c.CalificacionFinal > 6m);
                if (!calificaciones.Any())
                    return NotFound("No se encontraron calificaciones aprobatorias para esa materia.");

                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener calificaciones aprobatorias: {ex.Message}");
            }
        }

        [HttpGet("reprobatorias/materia/{idMateria}")]
        public async Task<IActionResult> ObtenerReprobatoriasPorMateria(string idMateria)
        {
            try
            {
                var calificaciones = await calificacionService.GetManyBy(c => c.IdMateria == idMateria && c.CalificacionFinal <= 6m);
                if (!calificaciones.Any())
                    return NotFound("No se encontraron calificaciones reprobatorias para esa materia.");

                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener calificaciones reprobatorias: {ex.Message}");
            }
        }

        [HttpGet("materia/{idMateria}")]
        public async Task<IActionResult> ObtenerPorMateria(string idMateria)
        {
            try
            {
                var calificaciones = await calificacionService.GetManyBy(c => c.IdMateria == idMateria);
                if (!calificaciones.Any())
                    return NotFound("No se encontraron calificaciones para esa materia.");

                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener calificaciones por materia: {ex.Message}");
            }
        }

        [HttpGet("ciclo/{idCiclo}")]
        public async Task<IActionResult> ObtenerPorCiclo(string idCiclo)
        {
            try
            {
                var calificaciones = await calificacionService.GetManyBy(c => c.IdCiclo != null && c.IdCiclo == idCiclo);
                if (!calificaciones.Any())
                    return NotFound("No se encontraron calificaciones para ese ciclo escolar.");

                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener calificaciones por ciclo escolar: {ex.Message}");
            }
        }

        [HttpGet("ciclo/{idCiclo}/materia/{idMateria}")]
        public async Task<IActionResult> ObtenerPorCicloYMateria(string idCiclo, string idMateria)
        {
            try
            {
                var calificaciones = await calificacionService.GetManyBy(c =>
                    c.IdCiclo != null && c.IdCiclo == idCiclo && c.IdMateria == idMateria);
                if (!calificaciones.Any())
                    return NotFound("No se encontraron calificaciones para ese ciclo y materia.");

                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener calificaciones por ciclo y materia: {ex.Message}");
            }
        }
    }
}
