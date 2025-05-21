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
    public class CicloEscolarController : ControllerBase
    {
        private readonly ICicloEscolar cicloEscolarService;

        public CicloEscolarController(ICicloEscolar cicloEscolarService)
        {
            this.cicloEscolarService = cicloEscolarService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var result = await cicloEscolarService.ObtenerCiclosEscolares();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener ciclos escolares: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(string id)
        {
            try
            {
                var ciclo = await cicloEscolarService.ObtenerCicloEscolarPorId(id);
                if (ciclo == null)
                    return NotFound("Ciclo escolar no encontrado");

                return Ok(ciclo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el ciclo escolar: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CicloEscolarDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await cicloEscolarService.AgregarCicloEscolar(dto);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(string id, [FromBody] CicloEscolarDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del cuerpo y la URL no coinciden");

            var response = await cicloEscolarService.ActualizarCicloEscolar(dto);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(string id)
        {
            // DTO temporal para eliminar solo con el id
            var dto = new CicloEscolarDTO(id, default, default, default, false);

            var response = await cicloEscolarService.EliminarCicloEscolar(dto);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("fecha-inicio/{fechaInicio}")]
        public async Task<IActionResult> ObtenerPorFechaInicio(DateTime fechaInicio)
        {
            try
            {
                var ciclos = await cicloEscolarService.GetManyBy(c => c.FechaInicio.Date == fechaInicio.Date);
                if (!ciclos.Any())
                    return NotFound("No se encontró ningún ciclo escolar con esa fecha de inicio.");

                var cicloDto = CicloEscolarMapper.FromEntity(ciclos.First());
                return Ok(cicloDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener ciclo escolar por fecha de inicio: {ex.Message}");
            }
        }

        // Obtener ciclo escolar actual
        [HttpGet("actual")]
        public async Task<IActionResult> ObtenerActual()
        {
            try
            {
                var ciclos = await cicloEscolarService.GetManyBy(c => c.EsActual);
                if (!ciclos.Any())
                    return NotFound("No hay ciclo escolar marcado como actual.");

                var cicloDto = CicloEscolarMapper.FromEntity(ciclos.First());
                return Ok(cicloDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener ciclo escolar actual: {ex.Message}");
            }
        }
    }
}
